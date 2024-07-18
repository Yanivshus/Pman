# Pman
A secure password manager, there is a need to remember master password and username, but the rest is on us.
## Technologies
Master password storing : Hash (sha-256) + random generated salt.
User passwords : Aes encryption, using the master password and salt as a key.
## Installation
Use visual studio to compile + need to have .NET framework installed.
Can use the local exe to run
## Soon to come...
2FA verification
Mail service if password is forgotten.
## License

[MIT](https://choosealicense.com/licenses/mit/)
