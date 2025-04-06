# How to Configure Blazor Server-Side using Docker

Small example on how to configure Blazor Server-Side using Docker.

## Starting

Instructions below will show your the way to get things working.

### Pre-requirements

***Self-Signed Certificate***

First, let us create our HTTPS certificate. You'll need to create this certificate to
enable HTTPS on your application, if you don't have one. For the sake of this example,
we'll create a development certificate using the following command:

```powershell
dotnet dev-certs https -ep "$env:USERPROFILE\.aspnet\https\aspnetcore.pfx" -p <YOUR_PASSWORD_HERE> --trust
```
The command above will create the certificate inside the folder _%USERPROFILE%\\.aspnet\https_ and
secure it using the specified password.

***The .env File***

Inside this project there is a `.env` file. You'll find some environment variables there that will be
used to create the Docker image and when you run the Docker Compose file.
You don't need to specify it when executing the Docker Compose command, because it will be automatically
loaded for you.

***In Development Mode***

In development mode, you don't need the certificate.

***Launch (on Docker)***

Run Docker Compose command in your Terminal like this:

```powershell
docker compose up -d
```

If you want more info about how to set a Dockerfile to your ASP.Net Core app,
please refer this documentations:

- [Run ASP.Net Core Docker HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md)
- [Hosting ASP.NET Core images with Docker Compose over HTTPS](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-9.0)

## Coding Styles

Nothing written into stone, use your ol'good common sense. But you can refere
to this page, if you like: [Common C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).

## Contribuition

Just me, at the moment.

## Authors

- **Marco Teixeira (marcoaoteixeira)** - _initial work_

## License

MIT

## Acknowledgement

- Hat tip to anyone whose code was used.