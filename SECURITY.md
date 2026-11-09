# Security policy

- Never request or store a user's Steam password.
- Never embed private API keys, client secrets, or server credentials in the UWP app.
- Use an appropriate Steam-supported delegated authentication flow where available.
- Keep refresh tokens and private credentials on a trusted backend, protected with access controls.
- Do not commit tokens, account identifiers, logs containing personal data, or local secret files.
- Report security issues privately to the repository owner rather than publishing exploit details.
