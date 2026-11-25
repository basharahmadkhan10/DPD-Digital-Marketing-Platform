# Multi-stage build for ASP.NET Core 10
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["DPDDigitalMarketing.csproj", "./"]
RUN dotnet restore "DPDDigitalMarketing.csproj"

COPY . .
RUN dotnet publish "DPDDigitalMarketing.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DPDDigitalMarketing.dll"]
