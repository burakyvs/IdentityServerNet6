#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["IdentityServerNet6/IdentityServerNet6.csproj", "IdentityServerNet6/"]
RUN dotnet restore "IdentityServerNet6/IdentityServerNet6.csproj"
COPY . .
WORKDIR "/src/IdentityServerNet6"
RUN dotnet build "IdentityServerNet6.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityServerNet6.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServerNet6.dll"]