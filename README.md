# Parking spot application in Ho Chi Minh City center
#### This repository holds an entire BE code in my Graduation Thesis 
## Table of Contents
- [Project-Brief](#Project-Brief)
  - [Architecture Description](#Description)  
- [Main-Flow](#Main-Flow)
- [Additional](#Additional)

## Project-Brief
- Pain Point:
  - Ho Chi Minh City is a vibrant metropolis with many entertainment options for families to enjoy. However, finding a parking spot can be challenging in the busy city center, which can affect the quality of the leisure time.
  - For parking lot owners who want to optimize their occupancy and revenue. Customers often flock to the most popular parking lots, but they may not find a spot or have to wait in long queues. This leads to frustration and wasted time for both customers and owners. On the other hand, some parking lots have plenty of available spaces, but they are not well-known or easily accessible. This results in underutilization and lost opportunities for owners
- Solution:
  - This is where our solution comes in: we help parking lot owners get more customers by connecting them with drivers who are looking for parking spaces nearby. Our solution uses a smart app that allows drivers to find, reserve, and pay for parking spaces in advance, and also provides real-time information on the availability and location of parking lots. By using our solution, parking lot owners can increase their visibility, occupancy, and revenue, while drivers can save time, money, and hassle
- This project is built based on a Clean Architecture
  ### Description:
  - Domain: This layer will represent Entities and business logic of all System
  - Application: This layer is responsible for implementing all Use Cases and has a dependency on the Domain layer
  - API: This layer will depend on the Application layer and will receive HTTP requests from the Client and direct them to the corresponding Use Case in the Application layer
  - Presentation: This layer is typically the user interface (UI) component of an application, such as a web interface, mobile application
  - Infrastructure: This layer will implement parts related to the database, communication with third-party services, and concrete implementation of interfaces defined at the application layer
  - The arrow pointing inward shows the dependency between layers. The deeper you go, the more abstract the inner components become, whereas the layers on the outside will be more detailed.
  - In addition, we have a project named Application.UnitTests to test only Application project
    
    ![Class Diagram - Clean (1) 1 (1)](https://github.com/ninehnineh/Capstone-BESource/assets/103179810/e6f3e25a-74ea-4881-8c24-c6f165dec2a6)
  
## Main-Flow
- On the 24th slide, we divide the main flow into 3 phases
  - Phase 1: The Manager login to the system and provides parking information for the operation of the parking
    - Brief
      - In this phase, we as a Manager or parking lot owners want to join and public these parking to our system
      - Fill parking information, set parking price, create a keeper (staff of parking)
      - Send request to Staff (staff of system)
    - [Manager Website](https://park-z-manager-web.vercel.app/)
    (Valid account: manager@gmail.com, password: 123)
    - [Source Repository](https://github.com/ParkZ-CapstoneProject/parkz-manager-website)
  - Phase 2: The staff of the system sends an approval request to the admin after confirming parking information and the admin decides request is valid or invalid for approval
    - Brief
      - In this phase, we as a Staff of the system will receive a request from parking lot owners (Managers), 
      - meeting with the Manager, validation provided information
      - Send approval request to Admin
    - [Staff Website](https://parkz-admin-website-eight.vercel.app)
    (Valid account: parkzstaff@gmail.com, password: 123)
    - [Source Repository](https://github.com/ParkZ-CapstoneProject/parkz-admin-website) We use the same source with admin and use Authozire to separate 2 roles
  - Phase 3: Customer booking process
    - Brief
      - In this phase, we as a Customer (drivers) access the application and book a desired slot in a specific parking
    - [Source Repository](https://github.com/ParkZ-CapstoneProject/parkz-mobile-app)
## Additional
- We have a [Admin Website](https://github.com/ParkZ-CapstoneProject/parkz-admin-website) for admin can manage system
  - For more information visit: [Source Repository](https://github.com/ParkZ-CapstoneProject/parkz-admin-website)  
  - [Admin Website](https://parkz-admin-website-eight.vercel.app/) (Valid account: admin@parkz.com, password: admin@@)
- We also have an application for Keeper (staff of parking) to manage parking slots (reason: our system needs to ensure the status (available or not) of parking slots in real life and in the system is synchronous)
  - For more information visit: [Source Repository](https://github.com/ParkZ-CapstoneProject/parkz_keeper_app)
- [Full Project URL](https://github.com/orgs/ParkZ-CapstoneProject/repositories)
- [Slide](https://www.canva.com/design/DAFj5jFOqNA/iux70mv-C_WX1w-vwzyyYA/edit?utm_content=DAFj5jFOqNA&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton)

#### © 2023 ChinhTruong
