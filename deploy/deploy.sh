#!/usr/bin/env bash
set -euo pipefail


REPO_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$REPO_ROOT"

echo "==> Working directory: $REPO_ROOT"

# ── 1. Start PostgreSQL + PostGIS via Docker Compose ─────────────────────────
echo "==> Starting PostgreSQL + PostGIS container..."
docker-compose -f deploy/docker-compose.yml up -d
echo "    Waiting 20 s for PostgreSQL to be ready..."
sleep 20

# ── 2. Run EF Core migrations ─────────────────────────────────────────────────
echo "==> Running database migrations..."
export PATH="$PATH:/root/.dotnet/tools"
ASPNETCORE_ENVIRONMENT=Production dotnet ef database update \
    --project Thefieldofdreams.Infrastructure \
    --startup-project Thefieldofdreams.API \
    --connection "Host=localhost;Port=5432;Database=Thefieldofdreams;Username=postgres;Password=Admin@2026"

# ── 3. Build and publish the .NET API ─────────────────────────────────────────
echo "==> Publishing .NET API..."
dotnet publish Thefieldofdreams.API/Thefieldofdreams.API.csproj \
    -c Release \
    -o /opt/thefieldofdreams-api

# Copy production appsettings override next to the published binary
cp deploy/appsettings.Production.json /opt/thefieldofdreams-api/appsettings.Production.json

# ── 3. Create / update the systemd service for the API ────────────────────────
echo "==> Configuring thefieldofdreams-api systemd service..."

# Create a dedicated service user if it does not already exist
if ! id -u thefieldofdreams &>/dev/null; then
    useradd --system --no-create-home --shell /usr/sbin/nologin thefieldofdreams
fi
chown -R thefieldofdreams:thefieldofdreams /opt/thefieldofdreams-api

# Write the environment file that holds sensitive credentials.
# Restrict permissions so only root and the service user can read it.
cat > /etc/thefieldofdreams-api.env << 'EOF'
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://localhost:5000
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=Thefieldofdreams;Username=postgres;Password=Admin@2026
EOF
chmod 640 /etc/thefieldofdreams-api.env
chown root:thefieldofdreams /etc/thefieldofdreams-api.env

# Copy the service unit file from the repo
cp deploy/thefield-api.service /etc/systemd/system/thefield-api.service

# ── 4. Enable and start the API service ───────────────────────────────────────
echo "==> Enabling and starting thefieldofdreams-api service..."
systemctl daemon-reload
systemctl enable thefield-api
systemctl restart thefield-api

# ── 5. Build Angular client ───────────────────────────────────────────────────
echo "==> Building Angular client..."
cd "$REPO_ROOT/ThefieldofdreamsClient"
npm install
npx ng build --configuration production
cd "$REPO_ROOT"

# ── 6. Deploy Angular built files to Nginx root ───────────────────────────────
echo "==> Deploying Angular files to /var/www/thefieldofdreams-client/..."
mkdir -p /var/www/thefieldofdreams-client
cp -r ThefieldofdreamsClient/dist/ThefieldofdreamsClient/browser /var/www/thefieldofdreams-client/
# Angular SSR emits index.csr.html instead of index.html — nginx needs index.html
if [ -f /var/www/thefieldofdreams-client/browser/index.csr.html ] && \
   [ ! -f /var/www/thefieldofdreams-client/browser/index.html ]; then
    cp /var/www/thefieldofdreams-client/browser/index.csr.html \
       /var/www/thefieldofdreams-client/browser/index.html
fi
chown -R www-data:www-data /var/www/thefieldofdreams-client

# ── 7. Configure Nginx ────────────────────────────────────────────────────────
echo "==> Configuring Nginx..."
cp deploy/nginx/thefieldofdreams.conf /etc/nginx/sites-available/thefieldofdreams

# Create symlink if it doesn't already exist
if [ ! -L /etc/nginx/sites-enabled/thefieldofdreams ]; then
    ln -s /etc/nginx/sites-available/thefieldofdreams /etc/nginx/sites-enabled/thefieldofdreams
fi

# Remove the default Nginx site
rm -f /etc/nginx/sites-enabled/default

# Test and reload Nginx
nginx -t
systemctl reload nginx

# ── 8. Done ───────────────────────────────────────────────────────────────────
echo ""
echo "╔══════════════════════════════════════════════════════════╗"
echo "║           thefieldofdreams System deployed successfully!          ║"
echo "╠══════════════════════════════════════════════════════════╣"
echo "║  Angular Client : http://109.199.102.179                 ║"
echo "║  API / Swagger  : http://109.199.102.179:5000            ║"
echo "╚══════════════════════════════════════════════════════════╝"
