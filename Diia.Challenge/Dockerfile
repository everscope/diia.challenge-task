#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Diia.Challenge/Diia.Challenge.csproj", "Diia.Challenge/"]
RUN dotnet restore "Diia.Challenge/Diia.Challenge.csproj"
COPY . .
WORKDIR "/src/Diia.Challenge"
RUN dotnet build "Diia.Challenge.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Diia.Challenge.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Diia.Challenge.dll"]