# Snapshotter

This project implements a client-server application designed to simplify file transfer operations typically used in red teaming scenarios. The client is written in C#, utilizing .NET Framework for file management and TCP communication. The server is implemented in Python, managing incoming connections and file reception.

## Features

- **ZIP Archive Creation**: The C# client recursively explores a specified directory and creates a ZIP archive containing all files and subdirectories.
  
- **Secure File Transfer**: Uses TCP socket connections to reliably transfer the ZIP file from the client to the server.

- **Thread Management**: The Python server employs threading to handle multiple client connections simultaneously, allowing for concurrent file transfers.

## Technologies Used

- **Client (C#)**: .NET Framework for file handling, compression with ZIP, and TCP communication.
  
- **Server (Python)**: Utilizes Python's `socket` module for connection handling and threading for concurrent client management.

## Usage

1. **Clone the Repository**: `git clone [<repository_url>](https://github.com/Vit8816/Snapshotter/)`
   
2. **Run the Server**: Execute `python server.py` to start the server listening on port 8800.

3. **Run the Client**: Launch the C# client application, providing the server's IP address and port as command-line arguments.

## How It Works

The C# client initiates by creating a compressed ZIP archive of the specified directory. It establishes a TCP connection with the Python server, sends the ZIP file over the network, and awaits acknowledgment from the server upon successful transfer.

The Python server listens for incoming connections on port 8800. Upon client connection, it spawns a new thread to handle each client separately. The server receives the ZIP file, saves it locally as `received_snapshot.rar`, and closes the connection once the transfer is complete.

## License

This project is licensed under the [MIT License](LICENSE).
