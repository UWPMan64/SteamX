# SteamX architecture

## Xbox client

The UWP app owns presentation, controller navigation, local non-sensitive preferences, and calls to public or authorized service endpoints.

## Backend (future)

A separately hosted backend may handle operations that require private credentials, token exchange, rate limiting, caching, and account-linked services. The backend must not act as a password collector or bypass Steam authentication.

## Integration boundaries

- Store discovery: public Store-facing endpoints, subject to change and rate limits.
- Steam account: use Steam-supported authentication/authorization mechanisms.
- Friends, owned games and achievements: availability depends on Steam API permissions, user privacy settings, and game-specific support.
- Cloud sync: Steam Cloud is not a generic third-party file-sync API. SteamX can implement its own sync for SteamX settings and metadata.
- Downloads: do not download or redistribute protected Steam game depots. Game installation and execution are not provided by this UWP starter.
- Streaming: a separate, explicitly authorized PC streaming client/server feature with its own protocol, network, and performance work.

## Security

Never ship private API keys or client secrets in the app package. Treat all data returned by remote endpoints as untrusted input.
