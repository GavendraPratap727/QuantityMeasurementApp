# Deployment Guide: Quantity Measurement App

## Backend Deployment on Render

### Prerequisites
- Render account (free tier available)
- GitHub repository with your code
- Database (Render provides free PostgreSQL)

### Step 1: Push Code to GitHub
1. Create a new repository on GitHub
2. Push your backend code to GitHub:
```bash
git init
git add .
git commit -m "Initial deployment setup"
git remote add origin https://github.com/yourusername/quantity-measurement-api.git
git push -u origin main
```

### Step 2: Set Up Database on Render
1. Go to Render Dashboard
2. Click "New +" -> "PostgreSQL"
3. Configure:
   - Name: `quantity-measurement-db`
   - Database Name: `quantitymeasurement`
   - User: `postgres`
   - Plan: Free
4. Click "Create Database"
5. Wait for database to be ready
6. Note the connection string from Render dashboard

### Step 3: Deploy Backend API
1. Go to Render Dashboard
2. Click "New +" -> "Web Service"
3. Connect your GitHub repository
4. Configure the service:
   - Name: `quantity-measurement-api`
   - Environment: `Docker`
   - Region: `Oregon` (or closest to you)
   - Plan: `Free`
5. Add Environment Variables:
   ```
   ConnectionStrings__DefaultConnection=[Your Render Database Connection String]
   Jwt__Key=YourSuperSecretKeyForProduction123456789
   Jwt__Issuer=QuantityMeasurementApp
   Jwt__Audience=QuantityMeasurementAppUsers
   Jwt__ExpiresInMinutes=60
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   ```
6. Click "Create Web Service"

### Step 4: Wait for Deployment
- Render will automatically build and deploy your API
- You can monitor the build logs
- Once deployed, your API will be available at: `https://quantity-measurement-api.onrender.com`

### Step 5: Test the API
1. Open your browser and go to: `https://quantity-measurement-api.onrender.com/swagger`
2. You should see the Swagger UI
3. Test the endpoints to ensure they work

## Frontend Deployment (Next Steps)

### Option 1: Netlify (Angular)
1. Build your Angular app for production:
```bash
cd frontendangular
npm run build --prod
```
2. Deploy the `dist/frontendangular` folder to Netlify

### Option 2: Vercel (Angular)
1. Install Vercel CLI
2. Run `vercel` in your Angular project folder
3. Follow the prompts

### Important: Update Frontend API URL
After deploying your backend, update the API URL in your frontend:
- In Angular: `frontendangular/src/app/services/quantity-measurement.service.ts`
- In HTML/JS: `QuantityMeasurementAppFrontend/app.js`

Change from:
```typescript
private apiBaseUrl = 'http://localhost:5263';
```
To:
```typescript
private apiBaseUrl = 'https://quantity-measurement-api.onrender.com';
```

## Environment Variables Explained

### Backend (Render)
- `ConnectionStrings__DefaultConnection`: Your PostgreSQL connection string
- `Jwt__Key`: Secret key for JWT tokens
- `Jwt__Issuer`: JWT issuer (your app name)
- `Jwt__Audience`: JWT audience (your users)
- `ASPNETCORE_ENVIRONMENT`: Set to Production
- `ASPNETCORE_URLS`: Set to `http://+:8080` for Render

### Frontend
- Update API base URL to point to your Render backend URL

## Troubleshooting

### Common Issues
1. **Database Connection Error**: Check your connection string format
2. **CORS Error**: Make sure your frontend URL is allowed
3. **Build Failure**: Check the build logs for specific errors
4. **500 Internal Server Error**: Check application logs

### Tips
- Always test locally before deploying
- Use Render's logs to debug issues
- Free tier has limitations (sleep after inactivity)
- Consider upgrading to paid plans for production use

## Next Steps
1. Deploy backend to Render
2. Update frontend API URL
3. Deploy frontend to Netlify/Vercel
4. Test the complete application
5. Set up custom domains if needed
