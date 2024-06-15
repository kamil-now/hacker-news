# Hacker News API

## Description
This project is a .NET 8 web application that provides an API to fetch the best stories from Hacker News.

## Prerequisites
[Docker](https://docs.docker.com/get-docker/) installed on your machine

The Docker Desktop installer usually sets up the Docker daemon as a Windows service, which starts automatically when you log in to your system. 

## Usage
To run the application using Docker Compose:

1. Open terminal and clone the repository:
   ```bash
   git clone https://github.com/kamil-now/hacker-news.git
   cd HackerNews
   ```
2. Build and start the application:
   ```bash 
   docker-compose up --build
   ```
3. The API will be accessible at http://localhost:5000/swagger/index.html.

## Design decisions
Because in the requirements it was stated that only one endpoint needs to be implemented, I decided to use .NET Minimal API.

To be able to efficiently service large numbers of requests without risking overloading of the API:
- **[redis](https://redis.io/)** is used as a caching layer to enhance application performance by storing frequently accessed data in memory,
- **[AspNetCoreRateLimit](https://github.com/stefanprodan/AspNetCoreRateLimit)** is used as a rate limiting solution designed to control the rate of requests that clients can make to the API based on IP address.

### Possible improvements
In case the solution would be further modified an extended with additional functionalities, as a first step I would add unit tests for mappers and services, ensuring no future regression to existing functionalities. I would also consider adding proper integration tests for rate limitter and cache. 

In case the solution were to be deployed to a production environment I would configure HTTPS redirection by exposing proper ports in Dockerfile and adding an SSL certificate. I would also configure the app to run in production mode and disable cache and rate limiter in development mode.

Depending on production environment requirements, what exactly is considered "large number of requests", what number of them would cause Hacker News API to become a bottleneck in a single API instance solution, and based on the available budget, I would consider more scalable solution. Dedicated database could be used to store the data received from Hacker News API, with mechanism that periodically updates the data making our API less dependent on the Hacker News API. Then I would consider using some kind of load balancer and Kubernetes to automatically scale the solution when needed. 
