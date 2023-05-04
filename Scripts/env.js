const fs = require('fs');
const path= require('path');

const dir = 'ESM.API';
const file = 'appsettings.json';

const content = `
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Jwt": {
    "Key": "${process.env.JWT_KEY}",
    "Issuer": "${process.env.JWT_ISSUER}",
    "Audience": "${process.env.JWT_AUDIENCE}",
    "Subject": "${process.env.JWT_SUBJECT}"
  },
  "ConnectionStrings": {
    "DefaultConnection": "${process.env.DEFAULT_CONNECTION_STRING}"
  }
}
`;

fs.access(dir, fs.constants.F_OK,(err) => {
  if (err) {
    console.log(`src doesn't exist, creating now`, process.cwd());
    fs.mkdir(dir, { recursive: true }, (err) => {
      if (err) throw err;
    })
  }

  try {
    fs.writeFileSync(dir + '/' + file, content);
    console.log('Created successfully in ', process.cwd());

    if (fs.existsSync(dir + '/' + file)) {
      console.log('File is created', path.resolve(dir + '/' + file));
      const str = fs.readFileSync(dir + '/' + file).toString();
      console.log(str);
    }
  } catch (error) {
    console.log(error);
    process.exit(1);
  }
})
