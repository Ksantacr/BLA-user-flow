FROM mcr.microsoft.com/dotnet/aspnet:8.0.3-alpine3.19 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0.203-alpine3.19 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BLA-user-flow.sln", "."]
COPY ["BLA.UserFlow.API/", "BLA.UserFlow.API/"]
COPY ["BLA.UserFlow.Application/", "BLA.UserFlow.Application/"]
COPY ["BLA.UserFlow.Core/", "BLA.UserFlow.Core/"]
COPY ["BLA.UserFlow.Infrastructure/", "BLA.UserFlow.Infrastructure/"]
RUN dotnet restore -v d
RUN dotnet build --no-restore -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish --no-restore -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS runner
USER root
RUN apk add --no-cache icu-libs

USER app
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BLA.UserFlow.API.dll"]