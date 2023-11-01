ARG CONFIGURATION
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /backend
EXPOSE 80
EXPOSE 443

ARG CONFIGURATION
RUN echo ${CONFIGURATION}
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ./*.sln ./
COPY . .

ARG CONFIGURATION
RUN echo ${CONFIGURATION}

RUN dotnet restore 
WORKDIR /src/Portal
RUN dotnet build "Portal.csproj" -c "$CONFIGURATION" -o /backend/build

ARG CONFIGURATION
RUN echo ${CONFIGURATION}

FROM build AS publish
RUN dotnet publish "Portal.csproj" -c "$CONFIGURATION" -o /backend/publish

FROM base AS final
WORKDIR /backend
COPY --from=publish /backend/publish .
ENV DOTNET_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "Portal.dll"]
