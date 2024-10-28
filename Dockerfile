FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TelegramChatBot/TelegramChatBot.csproj", "TelegramChatBot/"]
RUN dotnet restore "TelegramChatBot/TelegramChatBot.csproj"
COPY . .
WORKDIR "/src/TelegramChatBot"
RUN dotnet build "TelegramChatBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelegramChatBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramChatBot.dll"]