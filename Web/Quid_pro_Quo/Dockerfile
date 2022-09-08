#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Quid_pro_Quo/Quid_pro_Quo.csproj", "Quid_pro_Quo/"]
RUN dotnet restore "Quid_pro_Quo/Quid_pro_Quo.csproj"
COPY . .
WORKDIR "/src/Quid_pro_Quo"
RUN dotnet build "Quid_pro_Quo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Quid_pro_Quo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quid_pro_Quo.dll"]