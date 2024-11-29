# How to Run the Project with Docker

Follow these steps to run the project using Docker:

## 1. Open Terminal

Navigate to the root folder of your project where the `docker-compose.yml` file is located.

## 2. Build and Start Containers

Execute the following command:

```docker-compose up --build```


This command will build the necessary Docker images and start the containers as defined in your Docker Compose file.

During this process, Docker will automatically create the database and run any migrations required to set up the database schema.

Once the containers are up and running, you can access the Swagger API documentation by navigating to:

```http://localhost:8080/swagger/index.html```


## Coverage
![Screenshot 2024-11-29 at 18 04 40](https://github.com/user-attachments/assets/997a5de8-12b4-438a-b00d-0ac74a7f60f3)

