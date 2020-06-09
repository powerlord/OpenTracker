﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "Library expects string parameter.", Scope = "member", Target = "~M:OpenTracker.Models.AutoTracker.Start(System.String,System.Action{System.String,WebSocketSharp.LogLevel})")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "This is meant to catch errors thrown by the connector and websocket.", Scope = "member", Target = "~M:OpenTracker.Models.AutoTracker.InGameCheck")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "This is meant to catch errors thrown by the connector and websocket.", Scope = "member", Target = "~M:OpenTracker.Models.AutoTracker.MemoryCheck")]
[assembly: SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "Library expects string parameter.", Scope = "member", Target = "~M:OpenTracker.Models.AutotrackerConnectors.USB2SNESConnector.#ctor(System.String,System.Action{System.String,WebSocketSharp.LogLevel})")]
[assembly: SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "Library expects string parameter.", Scope = "member", Target = "~M:OpenTracker.Models.AutoTracker.Start(System.String)")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "This is meant to catch errors thrown by the connector and websocket.", Scope = "member", Target = "~M:OpenTracker.Models.AutotrackerConnectors.USB2SNESConnector.Read(System.UInt64,System.Byte[])~System.Boolean")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Future development required for localization.", Scope = "member", Target = "~M:OpenTracker.Models.AutotrackerConnectors.USB2SNESConnector.Read(System.UInt64,System.Byte[])~System.Boolean")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Future development required for localization.", Scope = "member", Target = "~M:OpenTracker.Models.AutotrackerConnectors.USB2SNESConnector.ConnectIfNecessary~System.Nullable{System.Boolean}")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "This is meant to catch errors thrown by the connector and websocket.", Scope = "member", Target = "~M:OpenTracker.Models.AutotrackerConnectors.USB2SNESConnector.ConnectIfNecessary~System.Nullable{System.Boolean}")]
[assembly: SuppressMessage("Reliability", "CA2008:Do not create tasks without passing a TaskScheduler", Justification = "TaskScheduler is not necessary for keepalive.", Scope = "member", Target = "~M:OpenTracker.Models.AutotrackerConnectors.USB2SNESConnector.ConnectIfNecessary~System.Nullable{System.Boolean}")]
[assembly: SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "ICollection interface is not necessary.", Scope = "type", Target = "~T:OpenTracker.Models.Utils.ObservableStack`1")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Class is purposely publicly accessible to allow for serialization.", Scope = "member", Target = "~P:OpenTracker.Models.SaveData.BossPlacements")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Class is purposely publicly accessible to allow for serialization.", Scope = "member", Target = "~P:OpenTracker.Models.SaveData.ItemCounts")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Class is purposely publicly accessible to allow for serialization.", Scope = "member", Target = "~P:OpenTracker.Models.SaveData.LocationSectionCounts")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Class is purposely publicly accessible to allow for serialization.", Scope = "member", Target = "~P:OpenTracker.Models.SaveData.LocationSectionMarkings")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Class is purposely publicly accessible to allow for serialization.", Scope = "member", Target = "~P:OpenTracker.Models.SaveData.PrizePlacements")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Class is purposely publicly accessible to allow for serialization.", Scope = "member", Target = "~P:OpenTracker.Models.SaveData.Connections")]
[assembly: SuppressMessage("Reliability", "CA2008:Do not create tasks without passing a TaskScheduler", Justification = "<Pending>", Scope = "member", Target = "~M:OpenTracker.Models.Sections.DungeonItemSection.UpdateAccessibility")]
