### Tiny Url Console App
This project allows the user to select the following options:
- Creating short URLs with associated long URLs
    - You have an option to enter a custom short URL
    - By default if custom option is invalid or not there then, the app will randomly generate one
- Deleting short URLs with associated long URLs
- Getting the long URL from a short URL
- Getting statistics on the number of times a short URL has been "clicked"

### Running steps
From top directory:
- dotnet build
- dotnet run --project TinyUrl/TinyUrl.csproj

### Unit Tests
- dotnet test