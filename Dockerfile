FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app

# Copy and restore as distinct layers
COPY *.sln ./
COPY ./src/Voidwell.UserManagement/*.csproj ./src/Voidwell.UserManagement/
COPY ./src/Voidwell.UserManagement.Api/*.csproj ./src/Voidwell.UserManagement.Api/
COPY ./src/Voidwell.UserManagement.Data/*.csproj ./src/Voidwell.UserManagement.Data/

RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime

# Copy the app
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

# Start the app
ENTRYPOINT dotnet Voidwell.UserManagement.Api.dll