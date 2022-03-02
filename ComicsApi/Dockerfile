FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ComicsApi/ComicsApi.csproj", "ComicsApi/"]
RUN dotnet restore "ComicsApi/ComicsApi.csproj"
COPY . .
WORKDIR "/src/ComicsApi"
RUN dotnet build "ComicsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ComicsApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ComicsApi.dll"]
