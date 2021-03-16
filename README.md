# Have I Been Breached

Have I Been Breached is a service that allows you to check if your email address is among those that have been breached sometime in the past. Of course they have to be added to our database first, but if you make a mistake you can delete your attempt and try again.

The service is written as an ASP<span>.</span>NET Core REST api with the following endpoints:

1. GET /breachedemails/:email
    - Allows the user to check if their email is in the database of breached emails
    - Responses: 200 OK, 404 NotFound or 400 Bad Request (if it's not an email)
2. POST /breachedemails
    - Allows the user to add an email to the database in case of a breach
    - Accepts: application/json
    - Responses: 201 Created, 409 Conflict or 400 Bad Request
3. DELETE /breachedemails/:email
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
4. Run it
    ```
    cd apps/WebApp
    dotnet run
    ```
5. Publish it
    ```
    cd apps/WebApp
    dotnet publish
    ```
