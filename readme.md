# Project Name
# Market Asset Price API

This project includes various APIs to fetch and update market asset data, including instruments, providers, exchanges, and bars. It also provides WebSocket functionality to stream data and displays it on the console as it appears. All data is saved into an SQLite database in the root of the project.

## Running the Application

When you run the application from Visual Studio, Swagger UI will automatically appear, providing an interface to interact with and test the API endpoints.

## Key Endpoints

### Instrument Controller

#### GET /api/Instrument/list-instruments

**Description**: Fetches all instrument data from an external API and updates the local database.

#### GET /api/Instrument/list-providers

**Description**: Retrieves all providers from the local database.

#### GET /api/Instrument/list-exchanges

**Description**: Retrieves all exchanges from the local database.

### WebSocket Controller

#### GET /api/WebSocket/start

**Description**: Starts streaming data through WebSocket and displays it on the console.

#### POST /api/WebSocket/stop

**Description**: Stops streaming data through WebSocket.

### Bars Controller

#### GET /api/Bars/list-bars

**Description**: Retrieves bar data based on count back parameters.

#### GET /api/Bars/date-range

**Description**: Retrieves bar data based on date range parameters.

#### GET /api/Bars/time-back

**Description**: Retrieves bar data based on time back parameters.

## Database

All data fetched and updated by the API is saved into an SQLite database located in the root of the project. Ensure the database file exists and is correctly configured in the project settings.

## How to Use

1. **Run the application from Visual Studio**: This will launch the app and open Swagger UI in your default browser.
2. **Navigate to the desired API endpoint** in Swagger UI.
3. **Execute the endpoint** to interact with the API and observe the WebSocket output on the console.