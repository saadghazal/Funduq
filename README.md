# Funduq - Hotel Reservation System

Funduq is a comprehensive hotel reservation system designed to streamline the booking process for users and provide hotel management with a powerful administrative panel (CMS) to manage their operations. This web application is built using **ASP.NET MVC** and **MySQL**, providing a robust and scalable solution for hotel reservations and management.

## Features

### User Side

Funduq offers an intuitive and user-friendly interface with the following key features:

1. **Hotel Room Booking**: Users can browse through available hotels and rooms, view details, and make bookings with ease.
2. **Invoice via Email**: Upon successful booking, users receive a detailed invoice via email.
3. **Add Testimonials**: Users can share their experiences by adding testimonials, which can be reviewed by the admin.
4. **Search for Hotels**: A comprehensive search functionality allows users to filter hotels by various criteria like location, price, and amenities.
5. **View Room Details**: Users can view detailed descriptions of rooms, including photos, amenities, and pricing.
6. **User Profile Management**: Users can manage their profiles, update personal information, and manage preferences.
7. **View Reservation History**: Users can see their past reservations and booking details.
8. **Secure Authentication**: The application offers secure user authentication and authorization mechanisms to protect user data and provide a safe experience.
9. **More Features**: The user side also includes additional features for enhanced usability and convenience.

### Admin Panel (CMS)

The admin panel provides hotel administrators with full control over hotel management, bookings, and user engagement:

1. **Dashboard Overview**: A comprehensive dashboard that shows key metrics, including new customers, monthly revenue, recent reservations, and more.
2. **Hotel Management**: Admins can add, update, and delete hotels and manage their details.
3. **Room Management**: Admins can add rooms to specific hotels, manage room details, pricing, and availability.
4. **Hotel Services Management**: Admins can add and manage various services offered by each hotel, like spa, gym, restaurant, etc.
5. **City Management**: Manage cities where hotels are located, making it easier to categorize and filter hotels based on location.
6. **Testimonial Management**: Admins can approve or reject user-submitted testimonials to ensure quality content.
7. **Reservation Filtering and Reporting**: Admins can filter reservations based on dates and generate detailed reservation reports in PDF or Excel formats.
8. **User Management**: Admins have the ability to manage user accounts, view user activities, and maintain the integrity of the platform.
9. **More Features**: The admin side is packed with more features to facilitate efficient hotel management.

## Technology Stack

- **Backend**: ASP.NET MVC
- **Frontend**: HTML, CSS, JavaScript, jQuery
- **Database**: MySQL
- **Email Service**: SMTP for sending emails
- **Report Generation**: PDF and Excel report generation for reservation data
- **Authentication**: Secure authentication and authorization implemented using ASP.NET Identity

## Installation and Setup

To set up the Funduq application locally, follow these steps:

1. **Clone the repository**:
   ```bash
   git clone https://github.com/saadghazal/Funduq.git
   cd Funduq
   ```

2. **Restore NuGet Packages**:
   Open the solution in Visual Studio, and restore the NuGet packages to install all dependencies.

3. **Setup Database**:
   - Create a MySQL database.
   - Run the SQL scripts provided in the `/Database` folder to create the necessary tables and seed data.
   - Update the connection string in `Web.config` to match your local database configuration.

4. **Run the Application**:
   Press `F5` in Visual Studio to build and run the application. The app will start on `http://localhost:xxxx` (port number may vary).
