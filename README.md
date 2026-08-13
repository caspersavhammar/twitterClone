# Real-Time Chat Application (C# / .NET · Avalonia)

A desktop chat application built with **C# and .NET 9** using the **Avalonia UI**
framework and the **MVVM** architecture. Two instances of the app connect to each
other directly over **TCP sockets**, allowing users to exchange messages in real time.

This project was developed for the course **TDDD49 – Programming in C# and .NET
Framework** at Linköping University, which focuses on object-oriented design,
the .NET platform and building event-driven desktop GUIs with data binding.

## Features

- **Peer-to-peer messaging** over raw TCP sockets (one instance hosts, the other connects).
- **Host / connect flow** – start a server on a chosen IP and port, or connect to a peer.
- **Live conversation view** with message history persisted to a local JSON store.
- **"Buzz"** – send an attention-grabbing alert to the other user.
- **Alert request/response dialogs** for connection and friend requests.
- Clean **MVVM** separation: `Models` (networking, message store), `ViewModels`
  (application logic and commands) and `Views` (Avalonia XAML).

## Tech stack

- C# · .NET 9
- Avalonia UI 11 (cross-platform desktop UI)
- CommunityToolkit.Mvvm (commands & observable properties)
- `System.Net.Sockets` (TCP networking)

## Project structure

```
tddd49/tddd49/
├── Models/        # NetworkManager (TCP), MessageManager (message storage)
├── ViewModels/    # MainWindow, AlertRequest — application logic & commands
├── Views/         # Avalonia XAML views (MainWindow, alerts, "Gnome"/Buzz)
├── db/            # Local JSON conversation history
└── Program.cs     # Application entry point
```

## Running the app

Requires the [.NET 9 SDK](https://dotnet.microsoft.com/download).

```bash
cd tddd49
dotnet run --project tddd49
```

Start one instance and press **Start** to host, then launch a second instance and
press **Connect** to join and start chatting.
