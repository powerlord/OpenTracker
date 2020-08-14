﻿using OpenTracker.Models.AccessibilityLevels;
using OpenTracker.Models.DungeonNodes;
using OpenTracker.Models.Dungeons;
using OpenTracker.Models.Items;
using OpenTracker.Models.KeyDoors;
using OpenTracker.Models.Locations;
using OpenTracker.Models.Modes;
using OpenTracker.Models.SequenceBreaks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OpenTracker.UnitTests.DungeonNodes
{
    [Collection("Tests")]
    public class PoDNodeTests
    {
        [Theory]
        [MemberData(nameof(Entry_To_PoD))]
        [MemberData(nameof(LobbyArena_To_PoD))]
        [MemberData(nameof(PoD_To_PastFirstRedGoriyaRoom))]
        [MemberData(nameof(PoD_To_LobbyArena))]
        [MemberData(nameof(PastFirstRedGoriyaRoom_To_LobbyArena))]
        [MemberData(nameof(BigKeyChestArea_To_LobbyArena))]
        [MemberData(nameof(HarmlessHellwayRoom_To_LobbyArena))]
        [MemberData(nameof(PastSecondRedGoriyaRoom_To_LobbyArena))]
        [MemberData(nameof(PastCollapsingWalkwayKeyDoor_To_LobbyArena))]
        [MemberData(nameof(LobbyArena_To_BigKeyChestArea))]
        [MemberData(nameof(LobbyArena_To_PastCollapsingWalkwayKeyDoor))]
        [MemberData(nameof(DarkBasement_To_PastCollapsingWalkwayKeyDoor))]
        [MemberData(nameof(HarmlessHellwayRoom_To_PastCollapsingWalkwayKeyDoor))]
        [MemberData(nameof(PastDarkMazeKeyDoor_To_PastCollapsingWalkwayKeyDoor))]
        [MemberData(nameof(PastCollapsingWalkwayKeyDoor_To_DarkBasement))]
        [MemberData(nameof(PastCollapsingWalkwayKeyDoor_To_HarmlessHellwayRoom))]
        [MemberData(nameof(PastCollapsingWalkwayKeyDoor_To_PastDarkMazeKeyDoor))]
        [MemberData(nameof(DarkMaze_To_PastDarkMazeKeyDoor))]
        [MemberData(nameof(PastDarkMazeKeyDoor_To_DarkMaze))]
        [MemberData(nameof(BigChestLedge_To_DarkMaze))]
        [MemberData(nameof(PastCollapsingWalkwayKeyDoor_To_BigChestLedge))]
        [MemberData(nameof(DarkMaze_To_BigChestLedge))]
        [MemberData(nameof(BigChestLedge_To_BigChest))]
        [MemberData(nameof(LobbyArena_To_PastSecondRedGoriyaRoom))]
        [MemberData(nameof(PastBowStatue_To_PastSecondRedGoriyaRoom))]
        [MemberData(nameof(PastSecondRedGoriyaRoom_To_PastBowStatue))]
        [MemberData(nameof(BossAreaDarkRooms_To_PastBowStatue))]
        [MemberData(nameof(PastBowStatue_To_BossAreaDarkRooms))]
        [MemberData(nameof(PastHammerBlocks_To_BossAreaDarkRooms))]
        [MemberData(nameof(BossAreaDarkRooms_To_PastHammerBlocks))]
        [MemberData(nameof(PastBossAreaKeyDoor_To_PastHammerBlocks))]
        [MemberData(nameof(PastHammerBlocks_To_PastBossAreaKeyDoor))]
        [MemberData(nameof(BossRoom_To_PastBossAreaKeyDoor))]
        [MemberData(nameof(PastBossAreaKeyDoor_To_BossRoom))]
        public void AccessibilityTests(
            DungeonNodeID id, ItemPlacement itemPlacement,
            DungeonItemShuffle dungeonItemShuffle, WorldState worldState,
            bool entranceShuffle, bool enemyShuffle, (ItemType, int)[] items,
            (SequenceBreakType, bool)[] sequenceBreaks, KeyDoorID[] keyDoors,
            AccessibilityLevel expected)
        {
            Mode.Instance.ItemPlacement = itemPlacement;
            Mode.Instance.DungeonItemShuffle = dungeonItemShuffle;
            Mode.Instance.WorldState = worldState;
            Mode.Instance.EntranceShuffle = entranceShuffle;
            Mode.Instance.EnemyShuffle = enemyShuffle;
            ItemDictionary.Instance.Reset();
            SequenceBreakDictionary.Instance.Reset();

            foreach (var item in items)
            {
                ItemDictionary.Instance[item.Item1].Current = item.Item2;
            }

            foreach (var sequenceBreak in sequenceBreaks)
            {
                SequenceBreakDictionary.Instance[sequenceBreak.Item1].Enabled =
                    sequenceBreak.Item2;
            }

            ((IDungeon)LocationDictionary.Instance[LocationID.PalaceOfDarkness]).DungeonDataQueue
                .TryPeek(out IMutableDungeon dungeonData);

            foreach (var keyDoor in dungeonData.SmallKeyDoors.Values)
            {
                keyDoor.Unlocked = keyDoors.Contains(keyDoor.ID);
            }

            foreach (var keyDoor in dungeonData.BigKeyDoors.Values)
            {
                keyDoor.Unlocked = keyDoors.Contains(keyDoor.ID);
            }

            Assert.Equal(expected, dungeonData.RequirementNodes[id].Accessibility);
        }

        public static IEnumerable<object[]> Entry_To_PoD =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDEntryTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.Retro,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDEntryTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.Inverted,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDEntryTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                },
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDEntryTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                },
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    true,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDEntryTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                },
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.Retro,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDEntryTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                },
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.Retro,
                    true,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDEntryTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                },
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.Inverted,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDEntryTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                },
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.Inverted,
                    true,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDEntryTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                },
            };

        public static IEnumerable<object[]> LobbyArena_To_PoD =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoD,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDFrontKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PoD_To_PastFirstRedGoriyaRoom =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastFirstRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDTest, 0),
                        (ItemType.Bow, 0),
                        (ItemType.Bottle, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.CameraUnlock, true),
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastFirstRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDTest, 1),
                        (ItemType.Bow, 0),
                        (ItemType.Bottle, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.CameraUnlock, true),
                        (SequenceBreakType.MimicClip, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastFirstRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDTest, 1),
                        (ItemType.Bow, 0),
                        (ItemType.Bottle, 1)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.CameraUnlock, false),
                        (SequenceBreakType.MimicClip, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastFirstRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    true,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDTest, 0),
                        (ItemType.Bow, 0),
                        (ItemType.Bottle, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.CameraUnlock, true),
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastFirstRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDTest, 1),
                        (ItemType.Bow, 0),
                        (ItemType.Bottle, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.CameraUnlock, true),
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                },
                new object[]
                {
                    DungeonNodeID.PoDPastFirstRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDTest, 1),
                        (ItemType.Bow, 0),
                        (ItemType.Bottle, 1)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.CameraUnlock, true),
                        (SequenceBreakType.MimicClip, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                },
                new object[]
                {
                    DungeonNodeID.PoDPastFirstRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDTest, 1),
                        (ItemType.Bow, 1),
                        (ItemType.Bottle, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.CameraUnlock, true),
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PoD_To_LobbyArena =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDFrontKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastFirstRedGoriyaRoom_To_LobbyArena =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastFirstRedGoriyaRoomTest, 0),
                        (ItemType.Hammer, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastFirstRedGoriyaRoomTest, 1),
                        (ItemType.Hammer, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastFirstRedGoriyaRoomTest, 1),
                        (ItemType.Hammer, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> BigKeyChestArea_To_LobbyArena =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBigKeyChestAreaTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBigKeyChestAreaTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> HarmlessHellwayRoom_To_LobbyArena =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDHarmlessHellwayRoomTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDHarmlessHellwayRoomTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastSecondRedGoriyaRoom_To_LobbyArena =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastSecondRedGoriyaRoomTest, 0),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastSecondRedGoriyaRoomTest, 1),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.MimicClip, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    true,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastSecondRedGoriyaRoomTest, 0),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastSecondRedGoriyaRoomTest, 1),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastSecondRedGoriyaRoomTest, 1),
                        (ItemType.Bow, 1)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastCollapsingWalkwayKeyDoor_To_LobbyArena =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDLobbyArena,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDCollapsingWalkwayKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> LobbyArena_To_BigKeyChestArea =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDBigKeyChestArea,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBigKeyChestArea,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBigKeyChestArea,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDBigKeyChestKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> LobbyArena_To_PastCollapsingWalkwayKeyDoor =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastCollapsingWalkwayKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastCollapsingWalkwayKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastCollapsingWalkwayKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDCollapsingWalkwayKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> DarkBasement_To_PastCollapsingWalkwayKeyDoor =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastCollapsingWalkwayKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDDarkBasementTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastCollapsingWalkwayKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDDarkBasementTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> HarmlessHellwayRoom_To_PastCollapsingWalkwayKeyDoor =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastCollapsingWalkwayKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDHarmlessHellwayRoomTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastCollapsingWalkwayKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDHarmlessHellwayRoomTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastCollapsingWalkwayKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDHarmlessHellwayRoomTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDHarmlessHellwayKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastDarkMazeKeyDoor_To_PastCollapsingWalkwayKeyDoor =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastCollapsingWalkwayKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastDarkMazeKeyDoorTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastCollapsingWalkwayKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastDarkMazeKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastCollapsingWalkwayKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastDarkMazeKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDDarkMazeKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastCollapsingWalkwayKeyDoor_To_DarkBasement =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDDarkBasement,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 0),
                        (ItemType.Lamp, 0),
                        (ItemType.FireRod, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkBasement, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkBasement,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1),
                        (ItemType.Lamp, 0),
                        (ItemType.FireRod, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkBasement, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkBasement,
                    ItemPlacement.Basic,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 0),
                        (ItemType.Lamp, 0),
                        (ItemType.FireRod, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkBasement, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkBasement,
                    ItemPlacement.Basic,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1),
                        (ItemType.Lamp, 0),
                        (ItemType.FireRod, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkBasement, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkBasement,
                    ItemPlacement.Basic,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1),
                        (ItemType.Lamp, 0),
                        (ItemType.FireRod, 1)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkBasement, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkBasement,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1),
                        (ItemType.Lamp, 0),
                        (ItemType.FireRod, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkBasement, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkBasement,
                    ItemPlacement.Basic,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1),
                        (ItemType.Lamp, 0),
                        (ItemType.FireRod, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkBasement, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkBasement,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1),
                        (ItemType.Lamp, 1),
                        (ItemType.FireRod, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkBasement, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkBasement,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1),
                        (ItemType.Lamp, 0),
                        (ItemType.FireRod, 1)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkBasement, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkBasement,
                    ItemPlacement.Basic,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1),
                        (ItemType.Lamp, 1),
                        (ItemType.FireRod, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkBasement, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastCollapsingWalkwayKeyDoor_To_HarmlessHellwayRoom =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDHarmlessHellwayRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDHarmlessHellwayRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDHarmlessHellwayRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDHarmlessHellwayKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastCollapsingWalkwayKeyDoor_To_PastDarkMazeKeyDoor =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastDarkMazeKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastDarkMazeKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                },
                new object[]
                {
                    DungeonNodeID.PoDPastDarkMazeKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDDarkMazeKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> DarkMaze_To_PastDarkMazeKeyDoor =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastDarkMazeKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDDarkMazeTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastDarkMazeKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDDarkMazeTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastDarkMazeKeyDoor_To_DarkMaze =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDDarkMaze,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastDarkMazeKeyDoorTest, 0),
                        (ItemType.Lamp, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    { 
                        (SequenceBreakType.DarkRoomPoDDarkMaze, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkMaze,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastDarkMazeKeyDoorTest, 1),
                        (ItemType.Lamp, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkMaze, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkMaze,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastDarkMazeKeyDoorTest, 1),
                        (ItemType.Lamp, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkMaze, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkMaze,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastDarkMazeKeyDoorTest, 1),
                        (ItemType.Lamp, 1)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkMaze, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> BigChestLedge_To_DarkMaze =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDDarkMaze,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBigChestLedgeTest, 0),
                        (ItemType.Lamp, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkMaze, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkMaze,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBigChestLedgeTest, 1),
                        (ItemType.Lamp, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkMaze, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkMaze,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBigChestLedgeTest, 1),
                        (ItemType.Lamp, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkMaze, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                },
                new object[]
                {
                    DungeonNodeID.PoDDarkMaze,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBigChestLedgeTest, 1),
                        (ItemType.Lamp, 1)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDDarkMaze, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastCollapsingWalkwayKeyDoor_To_BigChestLedge =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDBigChestLedge,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.BombJumpPoDHammerJump, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBigChestLedge,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.BombJumpPoDHammerJump, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBigChestLedge,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastCollapsingWalkwayKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.BombJumpPoDHammerJump, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                }
            };

        public static IEnumerable<object[]> DarkMaze_To_BigChestLedge =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDBigChestLedge,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDDarkMazeTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBigChestLedge,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDDarkMazeTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> BigChestLedge_To_BigChest =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDBigChest,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBigChestLedgeTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBigChest,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBigChestLedgeTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBigChest,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBigChestLedgeTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDBigChest
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> LobbyArena_To_PastSecondRedGoriyaRoom =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastSecondRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 0),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastSecondRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 1),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.MimicClip, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastSecondRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    true,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 0),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastSecondRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 1),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                },
                new object[]
                {
                    DungeonNodeID.PoDPastSecondRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDLobbyArenaTest, 1),
                        (ItemType.Bow, 1)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.MimicClip, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastBowStatue_To_PastSecondRedGoriyaRoom =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastSecondRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBowStatueTest, 0),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastSecondRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBowStatueTest, 1),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastSecondRedGoriyaRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBowStatueTest, 1),
                        (ItemType.Bow, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastSecondRedGoriyaRoom_To_PastBowStatue =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastBowStatue,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastSecondRedGoriyaRoomTest, 0),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastBowStatue,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastSecondRedGoriyaRoomTest, 1),
                        (ItemType.Bow, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastBowStatue,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastSecondRedGoriyaRoomTest, 1),
                        (ItemType.Bow, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> BossAreaDarkRooms_To_PastBowStatue =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastBowStatue,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBossAreaDarkRoomsTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastBowStatue,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBossAreaDarkRoomsTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastBowStatue_To_BossAreaDarkRooms =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDBossAreaDarkRooms,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBowStatueTest, 0),
                        (ItemType.Lamp, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDBossArea, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBossAreaDarkRooms,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBowStatueTest, 1),
                        (ItemType.Lamp, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDBossArea, false)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBossAreaDarkRooms,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBowStatueTest, 1),
                        (ItemType.Lamp, 0)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDBossArea, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.SequenceBreak
                },
                new object[]
                {
                    DungeonNodeID.PoDBossAreaDarkRooms,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBowStatueTest, 1),
                        (ItemType.Lamp, 1)
                    },
                    new (SequenceBreakType, bool)[]
                    {
                        (SequenceBreakType.DarkRoomPoDBossArea, true)
                    },
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastHammerBlocks_To_BossAreaDarkRooms =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDBossAreaDarkRooms,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastHammerBlocksTest, 0),
                        (ItemType.Hammer, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBossAreaDarkRooms,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastHammerBlocksTest, 1),
                        (ItemType.Hammer, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBossAreaDarkRooms,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastHammerBlocksTest, 1),
                        (ItemType.Hammer, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> BossAreaDarkRooms_To_PastHammerBlocks =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastHammerBlocks,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBossAreaDarkRoomsTest, 0),
                        (ItemType.Hammer, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastHammerBlocks,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBossAreaDarkRoomsTest, 1),
                        (ItemType.Hammer, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastHammerBlocks,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBossAreaDarkRoomsTest, 1),
                        (ItemType.Hammer, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastBossAreaKeyDoor_To_PastHammerBlocks =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastHammerBlocks,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBossAreaKeyDoorTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastHammerBlocks,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBossAreaKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastHammerBlocks,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBossAreaKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDBossAreaKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastHammerBlocks_To_PastBossAreaKeyDoor =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastBossAreaKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastHammerBlocksTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastBossAreaKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastHammerBlocksTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastBossAreaKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastHammerBlocksTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDBossAreaKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> BossRoom_To_PastBossAreaKeyDoor =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDPastBossAreaKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBossRoomTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastBossAreaKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBossRoomTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDPastBossAreaKeyDoor,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDBossRoomTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDBigKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };

        public static IEnumerable<object[]> PastBossAreaKeyDoor_To_BossRoom =>
            new List<object[]>
            {
                new object[]
                {
                    DungeonNodeID.PoDBossRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBossAreaKeyDoorTest, 0)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBossRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBossAreaKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[0],
                    AccessibilityLevel.None
                },
                new object[]
                {
                    DungeonNodeID.PoDBossRoom,
                    ItemPlacement.Advanced,
                    DungeonItemShuffle.Standard,
                    WorldState.StandardOpen,
                    false,
                    false,
                    new (ItemType, int)[]
                    {
                        (ItemType.PoDPastBossAreaKeyDoorTest, 1)
                    },
                    new (SequenceBreakType, bool)[0],
                    new KeyDoorID[]
                    {
                        KeyDoorID.PoDBigKeyDoor
                    },
                    AccessibilityLevel.Normal
                }
            };
    }
}
