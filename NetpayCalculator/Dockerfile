FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NetpayCalculator/NetpayCalculator.csproj", "NetpayCalculator/"]
RUN dotnet restore "NetpayCalculator/NetpayCalculator.csproj"
COPY . .
WORKDIR "/src/NetpayCalculator"
RUN dotnet build "NetpayCalculator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetpayCalculator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetpayCalculator.dll"]
