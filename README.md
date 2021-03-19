# Have I Been Breached

Have I Been Breached is a service that allows you to check if your email address is among those that have been breached sometime in the past. Of course they have to be added to our database first, but if you make a mistake you can delete your attempt and try again.

The service is written as an ASP<span>.</span>NET Core REST api with the following endpoints:

1. GET /breachedemails/{emailAddress}
    - Allows the user to check if their email is in the database of breached emails
    - Responses: 200 OK, 404 NotFound or 400 Bad Request (if it's not an email)
2. POST /breachedemails?emailAddress={emailAddress}
    - Allows the user to add an email to the database in case of a breach
    - Responses: 201 Created, 409 Conflict or 400 Bad Request
3. DELETE /breachedemails/{emailAddress}
    - Allows the user to remove an email that has been added in error
    - Responses: 200 OK or 400 Bad Request

To run the service yourself follow these steps:

1. Install the latest .NET 5.0 SDK (https://dotnet.microsoft.com/download/dotnet/5.0)
2. Clone it
    ```
    git clone https://github.com/dsedmak/haveibeenbreached.git
    ```
3. Build it
    ```
    cd haveibeenbreached/src
    dotnet build
    ```
4. Configure it
   - src/Apps/WebApp/appsettings.json
5. Create database (SQLite) and run migrations
   ```
   cd src/apps/WebApp
   dotnet tool install --global dotnet-ef
   dotnet ef database update
   ```
6. Run it
    ```
    cd src/apps/WebApp
    dotnet run
    ```
7. Use it
   - https://localhost:5001/index.html opens swagger which is fully functional