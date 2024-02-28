#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AmazingTech.InternSystem/AmazingTech.InternSystem.csproj", "AmazingTech.InternSystem/"]
RUN dotnet restore "AmazingTech.InternSystem/AmazingTech.InternSystem.csproj"
COPY . .
WORKDIR "/src/AmazingTech.InternSystem"
RUN dotnet build "AmazingTech.InternSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AmazingTech.InternSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ACCOUNT_DEFAULT_HTTP_PROTOCOL=https
ENTRYPOINT ["dotnet", "AmazingTech.InternSystem.dll"]
