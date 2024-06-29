import socket
import threading
import os

def handle_client(client_socket):
    file_size = client_socket.recv(8)
    file_size = int.from_bytes(file_size, byteorder='little')
    file_path = 'received_snapshot.rar'

    with open(file_path, 'wb') as f:
        remaining = file_size
        while remaining:
            data = client_socket.recv(min(8192, remaining))
            if not data:
                break
            f.write(data)
            remaining -= len(data)

    print(f"Received file: {file_path}")
    client_socket.close()

def main():
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind(("0.0.0.0", 8800))
    server.listen(5)
    print("Server listening on port 8800")

    while True:
        client_socket, addr = server.accept()
        print(f"Accepted connection from {addr}")
        client_handler = threading.Thread(target=handle_client, args=(client_socket,))
        client_handler.start()

if __name__ == "__main__":
    main()
