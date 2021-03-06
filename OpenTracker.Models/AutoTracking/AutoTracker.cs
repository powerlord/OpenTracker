﻿using OpenTracker.Models.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using WebSocketSharp;

namespace OpenTracker.Models.AutoTracking
{
    /// <summary>
    /// This is the class containing autotracking data and methods
    /// </summary>
    public class AutoTracker : Singleton<AutoTracker>, INotifyPropertyChanged
    {
        private byte? _inGameStatus;

        private bool InGame =>
            _inGameStatus.HasValue && _inGameStatus.Value > 0x05 && _inGameStatus.Value != 0x14;
        public ISNESConnector SNESConnector { get; }
        public Action<LogLevel, string> LogHandler { get; set; }

        public List<MemoryAddress> RoomMemory { get; } =
            new List<MemoryAddress>(592);
        public List<MemoryAddress> OverworldEventMemory { get; } =
            new List<MemoryAddress>(130);
        public List<MemoryAddress> ItemMemory { get; } =
            new List<MemoryAddress>(144);
        public List<MemoryAddress> NPCItemMemory { get; } =
            new List<MemoryAddress>(2);
        public List<MemoryAddress> DungeonItemMemory { get; } =
            new List<MemoryAddress>(6);
        public List<MemoryAddress> SmallKeyMemory { get; } =
            new List<MemoryAddress>(16);

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _raceIllegalTracking;
        public bool RaceIllegalTracking
        {
            get => _raceIllegalTracking;
            set
            {
                if (_raceIllegalTracking != value)
                {
                    _raceIllegalTracking = value;
                    PropertyChanged?.Invoke(
                        this, new PropertyChangedEventArgs(nameof(RaceIllegalTracking)));
                }
            }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public AutoTracker()
        {
            SNESConnector = SNESConnectorFactory.GetSNESConnector(HandleLog);

            for (int i = 0; i < 592; i++)
            {
                RoomMemory.Add(new MemoryAddress());

                if (i < 130)
                {
                    OverworldEventMemory.Add(new MemoryAddress());
                }

                if (i < 144)
                {
                    ItemMemory.Add(new MemoryAddress());
                }

                if (i < 16)
                {
                    SmallKeyMemory.Add(new MemoryAddress());
                }

                if (i < 6)
                {
                    DungeonItemMemory.Add(new MemoryAddress());
                }

                if (i < 2)
                {
                    NPCItemMemory.Add(new MemoryAddress());
                }
            }
        }

        /// <summary>
        /// Calls the LogHandler property and is passed to the connector to allow for log handling.
        /// </summary>
        /// <param name="logLevel">
        /// The log level of the event to be logged.
        /// </param>
        /// <param name="message">
        /// A string representing the log message.
        /// </param>
        private void HandleLog(LogLevel logLevel, string message)
        {
            LogHandler?.Invoke(logLevel, message);
        }

        /// <summary>
        /// Updates cached value of the SNES memory address that provides game status.
        /// </summary>
        public void InGameCheck()
        {
            if (SNESConnector != null && SNESConnector.Socket != null &&
                SNESConnector.Status != ConnectionStatus.Error &&
                SNESConnector.Read(0x7e0010, out byte inGameStatus))
            {
                _inGameStatus = inGameStatus;
            }
        }

        /// <summary>
        /// Updates cached values of a memory segment.
        /// </summary>
        /// <param name="segment">
        /// The memory segment to updated.
        /// </param>
        public void MemoryCheck(MemorySegmentType segment)
        {
            if (SNESConnector != null && SNESConnector.Socket != null &&
                SNESConnector.Status != ConnectionStatus.Error && InGame)
            {
                List<MemoryAddress> memory;
                ulong startAddress;

                switch (segment)
                {
                    case MemorySegmentType.Room:
                        {
                            startAddress = 0x7ef000;
                            memory = RoomMemory;
                        }
                        break;
                    case MemorySegmentType.OverworldEvent:
                        {
                            startAddress = 0x7ef280;
                            memory = OverworldEventMemory;
                        }
                        break;
                    case MemorySegmentType.Item:
                        {
                            startAddress = 0x7ef340;
                            memory = ItemMemory;
                        }
                        break;
                    case MemorySegmentType.NPCItem:
                        {
                            startAddress = 0x7ef410;
                            memory = NPCItemMemory;
                        }
                        break;
                    case MemorySegmentType.DungeonItem:
                        {
                            startAddress = 0x7ef434;
                            memory = DungeonItemMemory;
                        }
                        break;
                    case MemorySegmentType.SmallKey:
                        {
                            startAddress = 0x7ef4e0;
                            memory = SmallKeyMemory;
                        }
                        break;
                    default:
                        {
                            throw new ArgumentOutOfRangeException(nameof(segment));
                        }
                }

                byte[] buffer = new byte[memory.Count];

                if (SNESConnector.Read(startAddress, buffer))
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        memory[i].Value = buffer[i];
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator of devices to which can be connected.
        /// </summary>
        /// <returns>
        /// An enumerator of devices to which can be connected.
        /// </returns>
        public IEnumerable<string> GetDevices()
        {
            return SNESConnector.GetDevices();
        }

        /// <summary>
        /// Disconnects from the USB2SNES websocket, disposes of the connection, and
        ///  resets all memory addresses to null or 0.
        /// </summary>
        public void Stop()
        {
            SNESConnector.Disconnect();
            _inGameStatus = null;

            foreach (var address in RoomMemory)
            {
                address.Reset();
            }

            foreach (var address in OverworldEventMemory)
            {
                address.Reset();
            }

            foreach (var address in ItemMemory)
            {
                address.Reset();
            }

            foreach (var address in NPCItemMemory)
            {
                address.Reset();
            }

            foreach (var address in DungeonItemMemory)
            {
                address.Reset();
            }

            foreach (var address in SmallKeyMemory)
            {
                address.Reset();
            }
        }

        /// <summary>
        /// Returns the specified memory address.
        /// </summary>
        /// <param name="memorySegment">
        /// The memory segment of the address.
        /// </param>
        /// <param name="index">
        /// The index of the address.
        /// </param>
        /// <returns>
        /// The memory address.
        /// </returns>
        public static MemoryAddress GetMemoryAddress(MemorySegmentType memorySegment, int index)
        {
            return memorySegment switch
            {
                MemorySegmentType.Room => Instance.RoomMemory[index],
                MemorySegmentType.OverworldEvent => Instance.OverworldEventMemory[index],
                MemorySegmentType.Item => Instance.ItemMemory[index],
                MemorySegmentType.NPCItem => Instance.NPCItemMemory[index],
                MemorySegmentType.DungeonItem => Instance.DungeonItemMemory[index],
                MemorySegmentType.SmallKey => Instance.SmallKeyMemory[index],
                _ => throw new ArgumentOutOfRangeException(nameof(memorySegment))
            };
        }
    }
}
