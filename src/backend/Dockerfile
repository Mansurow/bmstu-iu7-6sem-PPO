ARG CONFIGURATION
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
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
WORKDIR /src/Anticafe
RUN dotnet build "Anticafe.csproj" -c "$CONFIGURATION" -o /app/build

ARG CONFIGURATION
RUN echo ${CONFIGURATION}

FROM build AS publish
RUN dotnet publish "Anticafe.csproj" -c "$CONFIGURATION" -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV DOTNET_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "Anticafe.dll"]
