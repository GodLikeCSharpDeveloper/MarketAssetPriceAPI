# Project Name

## Overview

This project provides an API service to get price information for specific market assets such as EUR/USD, GOOG, etc. The service is implemented as a REST API using .NET Core and follows best practices for API design. The data is stored in a SQLite database and the application runs in a Docker container.

## Prerequisites

- Docker
- .NET Core SDK

## Running the Application

### 1. Ensure Configuration

Verify that your configuration files, such as `appsettings.json`, are properly set up with the necessary connection strings and API keys.

### 2. Docker Commands

Navigate to the project root directory, where the Dockerfile and docker-compose.yml are located.

#### Build and Run Containers

Use the following command to build and run your Docker containers:

```bash
docker-compose up --build