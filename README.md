# 🧊 IceSync

Welcome to **IceSync**, a robust and scalable solution for synchronizing workflows using a clean architecture built on .NET.

## 🌟 Features 

- Intuitive UI for managing workflows.
- Efficient background synchronization service.
- Seamless integration with the Universal Loader API.
- Containerized services for easy deployment and scaling.

## 🚀 Getting Started 

### Prerequisites

1. **Docker**: IceSync runs on Docker for seamless deployment. If you haven't installed Docker yet, [get Docker here](https://www.docker.com/get-started).

### Running IceSync

2. **Terminal Setup**: 
   Open a terminal and navigate to the root directory of the solution.
   ```bash
   cd path_to_your_solution

### 📦 Building and Running IceSync 

3. **Build the Docker Containers**: 
   
   To build the necessary Docker containers, use the following command:
   ```bash
   docker-compose build
   
4. **Run IceSync**:
   Start the IceSync application using:
   ```bash
   docker-compose up
   
6. **Access the UI**:
   Open your browser and head over to [http://localhost:6001](http://localhost:6001).
   
8. **Connect to the SQL Server Container**:
   To directly access the SQL Server running inside the Docker container, use:
   ```bash
   docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P yourStrongPassword12!@

10. **Query the Database**:
    After connecting to the SQL Server instance, connect to the IceSyncLocal database and fetch data from the workflows table:
    ```bash
    >Use IceSyncLocal
    >select * from workflows
    >go
