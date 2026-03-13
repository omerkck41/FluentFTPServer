# GLOBAL GEMINI.md — Yazılım Şirketi Standardı
# Versiyon: 8.1 ULTIMATE
# .NET 10.0 LTS + Gemini 3 Pro (Agent Mode)
#
# ╔══════════════════════════════════════════════════╗
# ║  TEMEL KURAL Ve AJAN POLİTİKASI                  ║
# ║  Varsayılan  = Ücretsiz / Open Source            ║
# ║  [ÜCRETLI]   = Sadece kullanıcı talep ederse     ║
# ║  Bu kural istisnasız uygulanır.                  ║
# ║  MODEL: Gemini 3 Pro (Ajan Modu Varsayılan)      ║
# ╚══════════════════════════════════════════════════╝

---

## PROJE BAŞLANGIÇ PROTOKOLÜ

Her yeni projede bu 5 adım sırayla izlenir. Adım atlamak yasak. Ajan bu adımları otonom olarak terminal üzerinden yürütür.

```text
ADIM 1 → Projeyi anla
ADIM 2 → TIER belirle
ADIM 3 → Teknoloji Audit çalıştır
ADIM 4 → Proje GEMINI.md oluştur
ADIM 5 → /plan ile geliştirmeye başla
```

---

## ADIM 1 — PROJEYİ ANLA

Gemini Kullanıcıya sorar:

> "Projeyi kısaca anlat. Ne yapacak, kim kullanacak,
>  birden fazla müşteriye mi satılacak, ne kadar sürede bitmeli?"

---

## ADIM 2 — TIER BELİRLE

### 5 Soru

```text
S1. Projeyi kim kullanacak?
    (a) Sadece ben / küçük ekip
    (b) Şirket içi 5-50 kişi
    (c) Dış müşteriler
    (d) Geniş kullanıcı kitlesi

S2. Veri ne kadar kritik?
    (a) Test/deneme, kayıpsa sorun yok
    (b) Önemli ama yedeklenebilir
    (c) Kritik, kayıp kabul edilemez
    (d) Finansal veya yasal zorunluluk

S3. Tahmini kullanıcı sayısı?
    (a) 1-10    (b) 10-100    (c) 100-1000    (d) 1000+

S4. Proje süresi?
    (a) 1-3 gün    (b) 1-4 hafta    (c) 1-3 ay    (d) 3 ay+

S5. Güvenlik ihtiyacı?
    (a) Giriş yok
    (b) Basit kullanıcı girişi
    (c) Rol bazlı yetkilendirme
    (d) 2FA + Audit + Compliance
```

### Karar Mantığı

```text
Çoğunluk (a) → TIER 0
Çoğunluk (b) → TIER 1
Çoğunluk (c) → TIER 2
Çoğunluk (d) → TIER 3
S4=d, S2=d, S3=d → TIER 4 değerlendir
```

### Kullanıcıya Sunum Formatı

```text
📊 TIER [X] öneriyorum.

KULLANILACAKLAR:         KULLANILMAYACAKLAR:
✓ Clean Architecture     ✗ Kubernetes (gereksiz yük)
✓ EF Core + Dapper       ✗ RabbitMQ (tek servis)
✓ GitHub Actions         ✗ 2FA (iç kullanım)

Onaylıyor musun? (Terminalden E/H onayı beklenir)
```

---

## ADIM 3 — TEKNOLOJİ AUDİT

### Audit Raporu Formatı (Ajan Terminal Taraması Yapar)

```text
📦 TEKNOLOJİ AUDİT RAPORU

✅ Mevcut:      EF Core 10, Serilog
⚠️  Eksik:      FluentValidation, Hangfire, Polly
❌ Gerekmez:    Redis, RabbitMQ, SignalR
[ÜCRETLI] opt: Google Cloud Storage → MinIO tercih edildi

Eksik paketleri kurayım mı? (E/H)
```

Onay gelince Gemini:
1. `dotnet add package` ile terminalden kurar
2. `Program.cs` temel yapılandırmasını yapar
3. `appsettings.json` şablonunu oluşturur
4. `README.md` ve `GEMINI.md` günceller

### Audit Ek Soruları (Proje Tipine Göre)

```text
Multi-Tenant  → "Bu uygulama birden fazla müşteriye/şirkete satılacak mı?"
API Sunum     → "Dış sistemler bu API'yi kullanacak mı? Versiyon gerekli mi?"
Dosya Upload  → "Kullanıcılar dosya yükleyecek mi?"
Email         → "Sistem otomatik email gönderiyor mu?"
Bildirim      → "Anlık bildirim gerekiyor mu?"
Ödeme         → "Ödeme alınacak mı?"
Çok Dil       → "Birden fazla dil desteği gerekiyor mu?"
Raporlama     → "Karmaşık raporlar / dashboard var mı?"
Offline       → "İnternet bağlantısız çalışma gerekiyor mu?"
```

---

## TEKNOLOJİ HARİTASI

Varsayılan = Ücretsiz. [ÜCRETLI] = Sadece talep üzerine.

---

### CORE

| İhtiyaç | Paket | Not |
|---------|-------|-----|
| ORM | Microsoft.EntityFrameworkCore 10 | DB olan her proje |
| Hızlı sorgu | Dapper | Karmaşık rapor / 10k+ satır |
| Validation | FluentValidation.AspNetCore | Input alan her proje |
| Mapping | Mapster | DTO dönüşümü varsa |
| Mediator | MediatR | CQRS kullanılıyorsa |
| Logging | Serilog.AspNetCore | Her proje |
| Log sink (dev) | Serilog.Sinks.Seq (ücretsiz tier) | Geliştirme |
| Log sink (prod) | Serilog.Sinks.File / Google Cloud Logging | Prod projeler |
| Hata takibi | Sentry (ücretsiz tier) | TIER 1+ |
| API Docs | Scalar.AspNetCore | API olan her proje |
| Health Check | AspNetCore.HealthChecks | TIER 2+ |
| Global Exception | Custom middleware | Her proje |

---

### VERİTABANI

**Öneri sırası: SQLite → MySQL → PostgreSQL**

| DB | Provider | Maliyet |
|----|----------|---------|
| SQLite | Microsoft.EF.Sqlite | Ücretsiz |
| MySQL 8+ | Pomelo.EF.MySql | Ücretsiz |
| PostgreSQL | Npgsql.EF.PostgreSQL | Ücretsiz |
| MSSQL | Microsoft.EF.SqlServer | Dev/Express ücretsiz, Prod [ÜCRETLI] |
| MongoDB | MongoDB.Driver | Ücretsiz Community |
| Oracle | Oracle.EF.Core | [ÜCRETLI] |

**ORM Kuralı:**
- Yazma → EF Core (her zaman)
- Basit okuma → EF Core + AsNoTracking
- Karmaşık sorgu / rapor / 10k+ satır → Dapper

---

### MULTI-TENANT (Audit sorusu pozitifse)

Multi-tenant 3 stratejiyle uygulanır. Proje başında karar verilir.

```text
STRATEJİ 1 — Shared Database (Varsayılan, en basit)
─────────────────────────────────────────────────
Her tabloda TenantId kolonu.
EF Core global query filter ile otomatik filtreleme.
TenantId, JWT claim'den veya subdomain'den alınır.

Ne zaman: TIER 1-2, startup, maliyet önemli
Paket   : Finbuckle.MultiTenant (ücretsiz)
Risk    : Bir bug tüm tenant verilerini etkileyebilir

STRATEJİ 2 — Schema per Tenant
─────────────────────────────────────────────────
Her müşteri ayrı şemada (PostgreSQL güçlü).
Orta maliyet, iyi izolasyon.

Ne zaman: TIER 2-3, izolasyon önemli ama DB maliyeti yüksek istemiyorsa
Paket   : Finbuckle.MultiTenant + özel migration

STRATEJİ 3 — Database per Tenant
─────────────────────────────────────────────────
Her müşteriye ayrı veritabanı.
Tam izolasyon, en yüksek güvenlik.

Ne zaman: TIER 3+, kurumsal müşteriler, GDPR kritik, yüksek güvenlik
Paket   : Finbuckle.MultiTenant + dynamic connection string
Maliyet : Her tenant ayrı DB = altyapı maliyeti artar
```

**Tenant Tespiti (sırayla dene):**
```text
1. Subdomain  → musteri1.uygulama.com
2. Header     → X-Tenant-Id: musteri1
3. JWT Claim  → "tenant": "musteri1"
4. Path       → /musteri1/api/orders
```

**Multi-Tenant Güvenlik Kuralı:**
Her sorgu TenantId filtresi içermeli. EF Core global query filter bunu otomatik uygular. Manuel sorgu (Dapper) yazan ajan veya geliştiricinin `WHERE TenantId = @TenantId` koşulunu eklemesi PR aşamasında ajan tarafından denetlenir ve zorunludur.

---

### SOFT DELETE & VERİ YÖNETİMİ

Her projede baştan belirlenmesi gereken strateji.

```text
SEÇENEK 1 — Soft Delete (Varsayılan)
IsDeleted = true → EF Core global query filter ile otomatik gizle
DeletedAt, DeletedBy kolonları eklenir
Kim görebilir: Sadece Admin rolü

SEÇENEK 2 — Archive
Silinen kayıtlar ayrı tabloya (Orders → OrdersArchive) taşınır
Aktif tablo küçük kalır, performans iyi
Raporlamada arşiv de sorgulanabilir

SEÇENEK 3 — Hard Delete (GDPR "Unutulma Hakkı")
Gerçek silme — veri geri getirilemez
PII (kişisel veri) içeren tablolar için zorunlu
GDPR kapsamında kullanıcı talep ederse uygulanır
```

**BaseEntity (tüm entity'lerin türediği temel sınıf):**
```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }           // Soft delete
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public Guid? TenantId { get; set; }           // Multi-tenant (gerekirse)
}
```

**EF Core Global Query Filter:**
```csharp
modelBuilder.Entity<Order>()
    .HasQueryFilter(o => !o.IsDeleted
        && (!_multiTenantEnabled || o.TenantId == _currentTenantId));
```

---

### API VERSİYONLAMA (Dış API sunuluyorsa)

**Kütüphane:** `Asp.Versioning.Mvc` (ücretsiz, Microsoft resmi)

```text
YÖNTEM 1 — URL (Varsayılan, en yaygın)
GET /api/v1/orders
GET /api/v2/orders

YÖNTEM 2 — Header
GET /api/orders
X-API-Version: 2.0

YÖNTEM 3 — Query String (eski, önerilmez)
GET /api/orders?api-version=1.0
```

**Versiyon Yaşam Döngüsü:**
```text
Active     → Aktif, tam destek
Deprecated → Uyarı döneminde (6 ay), hâlâ çalışır
             Response header: Sunset: 2025-12-31
Retired    → Kaldırıldı, 410 Gone döner
```

**Kural:**
- v1 → v2 geçişinde v1 en az 6 ay daha aktif kalır
- Breaking change = yeni versiyon zorunlu
- Non-breaking ekleme = aynı versiyonda yapılabilir
- Deprecation bildirimi email + API response header ile yapılır

---

### AUTH & GÜVENLİK

| İhtiyaç | Varsayılan | Ücretli Alt. |
|---------|-----------|-------------|
| Kullanıcı yönetimi | ASP.NET Core Identity | — |
| JWT | Microsoft.AspNetCore.Authentication.JwtBearer | — |
| 2FA TOTP | OtpNet | — |
| 2FA Email OTP | MailKit + SMTP | — |
| 2FA SMS | Netgsm (TR) | [ÜCRETLI] Twilio |
| Rate Limiting | ASP.NET Core built-in | — |
| Secrets (dev) | dotnet user-secrets | — |
| Secrets (prod) | Environment Variables | [ÜCRETLI] Google Secret Manager |

**2FA Öneri Sırası:** TOTP → Email OTP → SMS OTP

---

### ARKA PLAN İŞLERİ

| İhtiyaç | Paket | Not |
|---------|-------|-----|
| Job scheduler | Hangfire.AspNetCore | Email, rapor, sync varsa zorunlu |
| Storage | Hangfire.MySql / InMemory | Seçilen DB'ye göre |
| Alternatif | Quartz.NET | Karmaşık zamanlama |
| Basit timer | BackgroundService (built-in) | Basit periyodik görev |

**Kural:** Email, bildirim, rapor üretme → Hangfire. Asla senkron yapma.

---

### HATA YÖNETİMİ & DAYANIKLILIK

| İhtiyaç | Paket | Ne Zaman |
|---------|-------|----------|
| Retry / Circuit Breaker | Polly | Dış API çağrısı olan her proje |
| HTTP Resilience | Microsoft.Extensions.Http.Polly | HTTP client kullanan her yerde |

**Polly Şablonu:**
```csharp
// Dış servis çağrısı olan her HTTP client için zorunlu
services.AddHttpClient<ISmsService, NetgsmSmsService>()
    .AddTransientHttpErrorPolicy(p =>
        p.WaitAndRetryAsync(3, attempt =>
            TimeSpan.FromSeconds(Math.Pow(2, attempt))))
    .AddTransientHttpErrorPolicy(p =>
        p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
```

---

### CACHE

| İhtiyaç | Çözüm | Ne Zaman |
|---------|-------|----------|
| Basit cache | IMemoryCache (built-in) | TIER 0-1, tek sunucu |
| Dağıtık cache | Redis — self-hosted (Docker: redis:alpine) | TIER 2+, çok sunucu |
| [ÜCRETLI] | Google Cloud Memorystore | Sadece talep üzerine |

---

### EMAIL

| İhtiyaç | Çözüm | Not |
|---------|-------|-----|
| Gönderme | MailKit | Her email ihtiyacı |
| Template | FluentEmail.Razor | HTML email şablonu |
| Servis | Brevo — 300 email/gün ücretsiz | Çoğu proje için yeterli |
| Test ortamı | Mailhog (Docker) | Gerçek email gönderme riski yok |
| [ÜCRETLI] | SendGrid, Mailjet Pro | Yüksek hacim |

---

### DOSYA YÖNETİMİ

| İhtiyaç | Çözüm | Ne Zaman |
|---------|-------|----------|
| Küçük proje | Local disk (IFormFile) | TIER 0-1 |
| Orta-büyük | MinIO — self-hosted, S3 uyumlu | TIER 2+ |
| [ÜCRETLI] | Google Cloud Storage, AWS S3 | Sadece talep üzerine |

**Dosya Güvenliği (her projede):**
- MIME tipi kontrolü (sadece uzantıya bakma)
- Boyut limiti zorunlu
- Orijinal dosya adı asla kaydetme → GUID kullan
- Public URL'de asla direkt servis etme

---

### TEST & TEST VERİSİ

| İhtiyaç | Paket | TIER |
|---------|-------|------|
| Unit | xUnit | 1+ |
| Mock | Moq | 1+ |
| Assertion | FluentAssertions | 1+ |
| Fake data | Bogus | 1+ — gerçekçi seed data |
| Integration | Microsoft.AspNetCore.Mvc.Testing | 2+ |
| DB container | Testcontainers | 2+ — gerçek DB test |
| E2E | Microsoft.Playwright | 2+ |
| Performance | NBomber | 3+ |
| Mutation | Stryker.NET | 3+ |

**Test Verisi Kuralı:**
- Unit test → Bogus ile üretilmiş fake data
- Integration test → Testcontainers ile gerçek DB
- Seed data → IDataSeeder interface + environment'a göre
- Production verisi test ortamına taşıma → anonimleştir (PII temizle)

---

### BİLDİRİM

| İhtiyaç | Çözüm | Not |
|---------|-------|-----|
| Realtime (web) | SignalR (built-in) | Anlık bildirim, canlı veri |
| Push (mobil) | Firebase Cloud Messaging | Ücretsiz, Google Native |
| In-app | Custom + SignalR | — |
| SMS | Netgsm (TR) | [ÜCRETLI servis, uygun fiyat] |

---

### UI KÜTÜPHANESİ

| Tip | Varsayılan | [ÜCRETLI] Alt. |
|-----|-----------|---------------|
| Web Blazor | MudBlazor | DevExpress, Telerik, Syncfusion |
| Grafik | ApexCharts.Blazor | DevExpress Charts |
| Masaüstü WPF | Community Toolkit | DevExpress WPF |
| Mobil MAUI | MAUI Community Toolkit | Telerik MAUI |

---

### DEVOPS & ALTYAPI

| İhtiyaç | Varsayılan | [ÜCRETLI] Alt. |
|---------|-----------|---------------|
| Container | Docker + Docker Compose | — |
| CI/CD | GitHub Actions (ücretsiz tier) | Google Cloud Build |
| Container registry | GitHub Container Registry | Google Artifact Registry |
| Orkestrasyon | K3s veya Docker Swarm | Google GKE (Kubernetes) |
| Monitoring | Prometheus + Grafana (self-hosted)| Datadog, New Relic |
| Log UI | Seq (dev, ücretsiz) | Google Cloud Logging |
| Uptime | Uptime Robot (ücretsiz) | Pingdom |

---

### MCP ARAÇLARI (Gemini Otonom Entegrasyonu)

**Her projede Gemini Ajanı şu MCP'leri kullanır:**
```text
GitHub MCP     → npx @modelcontextprotocol/server-github
Filesystem MCP → Doğrudan entegre (Workspace üzerinden okuma/yazma)
```

**Gereksinimi varsa otonom kurulur:**
```text
Database MCP   → npx @modelcontextprotocol/server-postgres  (ücretsiz)
Docker MCP     → npx @modelcontextprotocol/server-docker    (ücretsiz)
Playwright MCP → npx @modelcontextprotocol/server-playwright(ücretsiz)
Sentry MCP     → npx @sentry/mcp-server                     (ücretsiz tier)
Google Cloud MCP→ Yerleşik Gemini Cloud erişimi (Artifact Registry, vb.)
```

**MCP Güvenlik:**
- Her MCP için minimum izin
- Production DB → sadece read-only
- API key → environment variable (kod içine asla)
- Bilinmeyen kaynak MCP kurma

---

## TIER TANIMLARI

### TIER 0 — Statik
```text
Projeler : Landing page, portfolio, katalog, tanıtım sitesi
Stack    : Blazor Static / HTML+CSS+JS
DB       : Yok veya SQLite
Auth     : Yok
Test     : Manuel
CI/CD    : Yok veya otomatik GitHub Pages
Deploy   : GitHub Pages / Firebase Hosting / FTP
Süre     : 1-3 gün
```

### TIER 1 — Basit Uygulama
```text
Projeler : Randevu, form, blog, küçük stok, iç araçlar
Mimari   : Minimal Layered veya Vertical Slice
ORM      : EF Core (Dapper genellikle gerekmez)
DB       : SQLite veya MySQL
UI       : Blazor + MudBlazor
Auth     : Identity + JWT
Repo     : Generic Repository (UoW gerekmez)
Test     : Unit %60
CI/CD    : Opsiyonel
Deploy   : FTP + Actions / Shared Hosting / Hetzner CX11
Logging  : Serilog → dosya + Sentry ücretsiz
Süre     : 1-4 hafta
```

### TIER 2 — Orta Uygulama
```text
Projeler : CRM, ERP modülü, e-ticaret, SaaS başlangıcı
Mimari   : Clean Architecture
ORM      : EF Core + Dapper hibrit
DB       : MySQL veya PostgreSQL + Redis (self-hosted)
UI       : Blazor + MudBlazor
Auth     : Identity + JWT + Refresh + RBAC
Repo     : Generic + Specification + UoW
Test     : Unit %80 + Integration
CI/CD    : Zorunlu — GitHub Actions
Deploy   : Docker Compose + VPS (Hetzner)
2FA      : Opsiyonel
Logging  : Serilog + Seq
Monitoring: Uptime Robot + Sentry
Süre     : 1-3 ay
```

### TIER 3 — Büyük Uygulama
```text
Projeler : Büyük SaaS, multi-tenant platform, yüksek trafik
Mimari   : DDD + Clean Architecture
ORM      : EF Core (write) + Dapper (read) — CQRS
DB       : PostgreSQL + Redis + opsiyonel Elasticsearch
Queue    : RabbitMQ (self-hosted)
Auth     : Identity + 2FA zorunlu + Audit Log
Repo     : Tam gelişmiş + CQRS read/write ayrımı
Test     : Unit %85 + Integration + E2E + Perf + Mutation
CI/CD    : Tam pipeline + security scan
Deploy   : Docker + K3s veya Swarm / Google Cloud Run
Monitoring: Prometheus + Grafana (self-hosted)
Süre     : 3-12 ay
```

### TIER 4 — Kurumsal
```text
Not  : Mimar + DevOps + Güvenlik uzmanı gerektirir.
       Gemini yönlendirici rol oynar, kararları birlikte alır.
Süre : 6 ay+
```

---

## TIER YÜKSELTMESİ

```text
/tier-upgrade:
1. Mevcut vs hedef TIER farkını listele
2. Teknoloji Audit ile eksikleri tespit et
3. Kademeli plan oluştur (big bang değil)
4. Her adım ayrı sprint/PR

Örnek TIER 1 → TIER 2:
  Sprint 1: Docker Compose
  Sprint 2: GitHub Actions CI/CD
  Sprint 3: Redis (self-hosted)
  Sprint 4: Integration testler
  Sprint 5: Monitoring
```

---

## MİMARİ KARAR AĞACI

```text
CRUD ağırlıklı, küçük, hızlı?
  → Vertical Slice

Standart kurumsal, orta karmaşıklık?
  → Clean Architecture (varsayılan)

Büyük proje, karmaşık iş kuralları?
  → DDD + Clean Architecture

API öncelikli, çoklu dış entegrasyon?
  → Hexagonal (Ports & Adapters)
```

---

## VERİTABANI KARAR AĞACI

```text
Mobil / masaüstü / offline?       → SQLite
Küçük-orta web, maliyet önemli?   → MySQL
Büyük veri, JSON, GIS?            → PostgreSQL
Windows/Azure ekosistemi?         → MSSQL [Dev ücretsiz, Prod ÜCRETLI]
Esnek şema, event/log store?      → MongoDB
Kullanıcı açıkça istedi?          → Oracle [ÜCRETLI]
İki DB gerekli?                   → Hibrit (örn: MySQL + MongoDB log)
```

---

## HOT-FIX PROSEDÜRÜ

```text
Production'da bug bulundu:
1. main'den hotfix/[isim] branch aç
2. Düzelt ve test et (odaklı, minimal değişiklik)
3. Fast-track: build → test → staging → prod
4. Hedef: 2 saat içinde production
5. develop'a da merge et
6. CHANGELOG güncelle
7. Müşteriye bildirim gönder (TIER 2+)
```

---

## ONBOARDING — YENİ GELİŞTİRİCİ VEYA YENİ AJAN SESSION'I

```text
Yeni geliştirici veya Yeni Gemini Oturumu eklendi:
1. Global GEMINI.md oku (bu dosya)
2. Proje GEMINI.md oku
3. README.md oku (kurulum adımları)
4. Ajan docker-compose up ile ortamı kurar
5. Testleri çalıştır (hepsi geçmeli)
6. İlk görev: küçük bir bug fix veya test yazma
7. İlk PR'da Gemini otomatik code review yapar
```

---

## MÜŞTERI TESLİM & SLA

```text
TIER 0: Deploy linki + kısa kullanım notu
TIER 1: README + kurulum + kullanım kılavuzu
TIER 2: API docs + DB şeması + kurulum + test raporu + bakım notu
TIER 3: Tam paket:
        - ADR (mimari kararlar ve gerekçeleri)
        - API referans dokümantasyonu
        - DB şeması ve açıklamaları
        - Kurulum ve deployment rehberi
        - Test raporu (coverage + security)
        - Monitoring dashboard
        - SLA tanımı
        - Bakım ve destek rehberi
        - Lisans ve bağımlılık listesi
```

**SLA Şablonu (TIER 3):**
```text
Uptime hedefi   : %99.5
Response time   : p95 < 500ms
Bug fix         : Kritik 24 saat / Major 1 hafta / Minor sonraki sprint
Bildirim        : Production kesintisi → 30 dakika içinde müşteriye
Bakım penceresi : Pazar 02:00-04:00
```

---

## TÜM TIER'LARDA ZORUNLU

```text
✓ Git repository + .gitignore
✓ HTTPS (production'da HTTP yasak)
✓ README.md
✓ Error logging (en az dosya log)
✓ Semantic versioning (v1.0.0)
✓ Code review (Gemini otomatik terminalde çalıştırır)
✓ Input validation (kullanıcıdan veri alınıyorsa)
✓ Backup (DB varsa, TIER 0 hariç)
✓ Proje GEMINI.md
```

---

## KOD STANDARTLARI

```text
✓ async/await — tüm I/O işlemlerinde
✓ Public üyelere XML dokümantasyon
✓ nullable enable
✓ CancellationToken — async metodlarda
✓ Anlamlı isimler (kısaltma yasak)
✓ Magic number yasak → const kullan
✓ Her entity BaseEntity'den türemeli

✗ static class (DI bozulur)
✗ Thread.Sleep → Task.Delay kullan
✗ .Result / .Wait() → deadlock riski
✗ async void → event handler hariç
✗ throw ex → throw kullan
✗ Console.WriteLine → logger kullan
✗ Hardcoded secret → env variable
✗ Ücretli araç → kullanıcı onayı olmadan kurma
✗ Multi-tenant sorguda TenantId filtresi atlamak
```

---

## GÜVENLİK (TIER BAZLI)

| Konu | T0 | T1 | T2 | T3 |
|------|----|----|----|----|
| HTTPS | ✓ | ✓ | ✓ | ✓ |
| Input Validation | — | ✓ | ✓ | ✓ |
| Auth (JWT) | — | ✓ | ✓ | ✓ |
| RBAC | — | Basit | ✓ | ✓ |
| Soft Delete | — | ✓ | ✓ | ✓ |
| Rate Limiting | — | — | ✓ | ✓ |
| 2FA | — | — | Opt | ✓ |
| Audit Log | — | — | ✓ | ✓ |
| OWASP Scan | — | — | ✓ | ✓ |
| Pentest | — | — | — | ✓ |

---

## GİT STRATEJİSİ

```text
TIER 0-1: main branch yeterli
TIER 2+ : main / develop / feature/xxx / hotfix/xxx

Conventional Commits (TIER 2+):
  feat / fix / refactor / test / docs / security / perf
```

---

## DEPLOY (TIER BAZLI)

```text
TIER 0: GitHub Pages / Firebase Hosting / FTP
TIER 1: FTP + GitHub Actions / Shared Hosting / Hetzner CX11
TIER 2: Docker Compose + VPS / GitHub Actions
        [ÜCRETLI opt] Google Cloud Run
TIER 3: Docker + K3s/Swarm
        [ÜCRETLI opt] Google GKE (Kubernetes)
TIER 4: Kullanıcı ile birlikte karar
```

---

## LOCALİZATION (TIER BAZLI)

```text
TIER 0-1: Gerekmez
TIER 2  : Opsiyonel — çok dilli gereksinim varsa
TIER 3  : Zorunlu — uluslararası pazar

Yapı: ASP.NET Core Localization (built-in)
      Resource dosyaları (.resx)
      DB içerik → Translation tablosu
```

---

## MONİTORİNG (TIER BAZLI)

```text
TIER 0: Uptime Robot ücretsiz tier
TIER 1: Uptime Robot + Serilog dosya + Sentry ücretsiz
TIER 2: Serilog + Seq (self-hosted) + Sentry
TIER 3: Prometheus + Grafana (self-hosted) + alerting
        [ÜCRETLI opt] Datadog, Google Cloud Logging
```

---

## SKILLS KATALOĞU (Gemini Otonom Komutları)

| Skill | Açıklama | Min TIER |
|-------|----------|----------|
| `/plan` | Requirement, ADR, User Story, Şema | Tümü |
| `/tier-select` | TIER belirleme — 5 soru | Tümü |
| `/tier-upgrade` | Kademeli TIER yükseltme planı | Tümü |
| `/tech-audit` | Teknoloji audit + kurulum | Tümü |
| `/tdd-cycle` | RED → GREEN → IMPROVE | 1+ |
| `/solid-review` | SOLID analiz + refactor | 1+ |
| `/ddd-review` | Aggregate, Bounded Context | 3+ |
| `/module-generator` | TIER'a uygun CRUD modülü | 1+ |
| `/repo-generator` | Repo + Spec + UoW (TIER bazlı) | 1+ |
| `/api-endpoint` | Endpoint + Auth + Docs | 1+ |
| `/api-versioning` | API versiyon kurulum + strateji | 2+ |
| `/multi-tenant` | Tenant stratejisi + implementasyon | 2+ |
| `/soft-delete` | Global query filter + strateji | 1+ |
| `/dapper-query` | Performans kritik sorgu | 2+ |
| `/db-migration` | Migration + rollback | 1+ |
| `/security-scan` | OWASP + dependency audit | 2+ |
| `/2fa-setup` | TOTP / Email / SMS 2FA | 2+ |
| `/localization-setup` | Çoklu dil kurulum | 2+ |
| `/background-job` | Hangfire kurulum + job | 1+ |
| `/file-storage` | Upload + local/MinIO | 1+ |
| `/performance-audit` | N+1, cache, index analizi | 2+ |
| `/notification-setup` | SignalR / Push / Email | 2+ |
| `/blazor-component` | MudBlazor bileşen + test | 1+ |
| `/maui-page` | MAUI MVVM + offline sync | 1+ |
| `/docker-setup` | Dockerfile + Compose | 2+ |
| `/ci-pipeline` | GitHub Actions pipeline | 1+ |
| `/mcp-setup` | MCP kurulum + yapılandırma | Tümü |
| `/monitoring-setup` | TIER uygun monitoring | 1+ |
| `/release-prep` | Versiyon + CHANGELOG | Tümü |
| `/hotfix` | Hot-fix prosedürü | 1+ |
| `/onboarding-doc` | Proje dokümantasyonu + kurulum | Tümü |

---

## HOOKS (Gemini Ajan Otonomisi)

| Hook | T0 | T1 | T2+ |
|------|----|----|-----|
| session-start (dosyaları oku) | ✓ | ✓ | ✓ |
| pre-commit (build) | ✓ | ✓ | ✓ |
| pre-commit (test) | — | ✓ | ✓ |
| coverage-check | — | ✓ | ✓ |
| security-gate | — | — | ✓ |
| migration-check | — | ✓ | ✓ |
| session-end (özet + commit) | ✓ | ✓ | ✓ |
| error-catch (terminal hatası fix) | ✓ | ✓ | ✓ |

---

## PROJE GEMINI.MD ŞABLONU

ADIM 4'te her proje için bu şablon doldurulur.
Geliştirme boyunca canlı tutulur, sprint sonlarında güncellenir.

```markdown
# PROJE GEMINI.md — [Proje Adı]
# Oluşturulma: [Tarih] | Son güncelleme: [Tarih]

## Proje Özeti
[Bir paragraf: ne yapıyor, kim kullanıyor, temel amacı]

## TIER: [0 / 1 / 2 / 3 / 4]

## Tech Stack
- Mimari  : [Vertical Slice / Clean Arch / DDD]
- Backend : .NET 10 — ASP.NET Core
- ORM     : EF Core 10 [+ Dapper — karmaşık sorgular]
- DB      : [MySQL / PostgreSQL / SQLite]
- Cache   : [Memory Cache / Redis]
- UI      : [Blazor Server + MudBlazor]
- Auth    : [Identity + JWT]
- Logging : Serilog → [dosya / Seq]
- Test    : xUnit + Moq + FluentAssertions + Bogus
- Deploy  : [FTP / Docker Compose + VPS / K3s]
- CI/CD   : [Yok / GitHub Actions]

## Ekstra Paketler (Audit Sonrası)
- [FluentValidation — form validation]
- [Hangfire — email gönderme]
- [Polly — dış API retry]
- [...]

## Ortam Bilgileri
- Dev     : localhost:5000 | DB: localhost:3306/proje_dev
- Staging : staging.domain.com | DB: staging-server/proje_stg
- Prod    : domain.com | DB: prod-server/proje_prod

## Multi-Tenant
[Yok / Shared DB / Schema / DB per Tenant]
Tenant tespiti: [Subdomain / Header / JWT Claim]

## API Versiyonlama
[Yok / URL bazlı v1 / Header bazlı]

## Mimari Kararlar (ADR)
ADR-001: [Karar] — [Gerekçe] — [Tarih]
ADR-002: [Karar] — [Gerekçe] — [Tarih]

## Özel İş Kuralları
- [Kural 1]
- [Kural 2]

## DB Şeması Özeti
- [Tablo1]: [açıklama]
- [Tablo2]: [açıklama]

## Bilinen Kısıtlar / Teknik Borç
- [...]

## Sprint Geçmişi
Sprint 1: [Tamamlanan özellikler]
Sprint 2: [Tamamlanan özellikler]
```

---

## ENVIRONMENT YÖNETİMİ

Her projede baştan kurulur, sonradan düzeltmek zordur.

### Ortam Yapısı

```text
Development → Local geliştirme
Staging     → Production kopyası, son test
Production  → Gerçek kullanıcılar
```

### appsettings Yapısı

```text
appsettings.json                  ← Ortak ayarlar (secret YOK)
appsettings.Development.json      ← Local geliştirme
appsettings.Staging.json          ← Staging ortamı
appsettings.Production.json       ← Production (secret YOK, env var'dan okunur)
```

### Secret Yönetimi (Ortama Göre)

```text
Development → dotnet user-secrets
              Ajan otomatik kurar: dotnet user-secrets set "Db:Password" "localpass"

Staging     → VPS'te environment variable
              export ConnectionStrings__Default="Server=..."

Production  → VPS'te environment variable (aynı yöntem)
              [ÜCRETLI opt] Google Secret Manager

CI/CD       → GitHub Actions Secrets
              secrets.DB_PASSWORD → env variable olarak inject edilir
```

### .gitignore Zorunluları

```text
# Secrets — asla commit edilmez
appsettings.*.json       # Development/Staging/Production overrides
*.pfx
*.p12
.env
.env.*
user-secrets.json

# Build çıktıları
bin/
obj/
publish/

# IDE
.vs/
.idea/
*.user
```

### appsettings.json Şablonu

```json
{
  "ConnectionStrings": {
    "Default": ""
  },
  "Jwt": {
    "Key": "",
    "Issuer": "",
    "Audience": "",
    "ExpiryMinutes": 60
  },
  "Serilog": {
    "MinimumLevel": "Information"
  },
  "Sentry": {
    "Dsn": ""
  }
}
```

**Kural:** `appsettings.json` içinde hiçbir zaman gerçek değer olmaz.
Tüm değerler environment variable veya user-secrets'tan gelir.

---

## DB MİGRASYON STRATEJİSİ

### Ortama Göre Migration Yöntemi

```text
TIER 1 — Development ortamı:
  Startup'ta otomatik (sadece dev için kabul edilebilir)
  if (app.Environment.IsDevelopment())
      await db.Database.MigrateAsync();

TIER 2 — CI/CD pipeline içinde (önerilen):
  - Build sonrası, deploy öncesi çalışır
  - GitHub Actions adımı:
    dotnet ef database update --connection "${{ secrets.DB_CONN }}"
  - Hata → deploy durur, rollback tetiklenir

TIER 3 — Manuel SQL script (kurumsal):
  dotnet ef migrations script --idempotent -o migration.sql
  DBA script'i inceler ve çalıştırır
  Audit log'a kaydedilir
```

### Rollback Stratejisi

```text
Seçenek 1 — Down migration (basit):
  dotnet ef database update [önceki migration adı]
  Dikkat: Veri kaybı riski, production'da dikkatli kullan

Seçenek 2 — Önceki sürümü deploy et:
  Git tag ile önceki sürüme dön
  DB değişmeden kalır (migration geri alınmadı)
  En güvenli yol

Seçenek 3 — Hotfix migration:
  Yeni bir "düzeltici" migration yaz
  Veriyi koruyarak sorunu çöz
```

### Migration Kuralları

```text
✓ Her migration tek bir sorumluluğa sahip olmalı
✓ Migration adları açıklayıcı olmalı (AddOrderStatusColumn)
✓ Production migration'ı önce staging'de test et
✓ Büyük tablo değişikliklerinde zero-downtime migration kullan
✗ Production'da dotnet ef database drop asla
✗ Shared DB'de migration'ı koordinatsız çalıştırma
✗ Migration içine iş mantığı yazma
```

---

## SPRINT DÖNGÜSÜ & DONE TANIMI

### Sprint Yapısı (TIER Bazlı)

```text
TIER 0   : Sprint yok — task listesi yeterli
TIER 1   : 1 haftalık sprint
TIER 2+  : 2 haftalık sprint (önerilen)
```

### Sprint Başında (Planning)

```text
1. Bir önceki sprint'in bitmemiş işlerini değerlendir
2. Bu sprint'e alınacak User Story'leri seç
3. Her Story'yi task'lara böl
4. Tahmin ver (saat veya story point)
5. Sprint hedefini tek cümleyle yaz
```

### Sprint İçinde (Daily)

```text
Her gün başında 3 soru:
  - Dün ne yaptım?
  - Bugün ne yapacağım?
  - Engelim var mı?

Gemini ile her oturum başında:
  - session-start hook çalışır
  - Proje GEMINI.md okunur
  - Ajan Terminal hatalarını tespit eder
  - Kaldığı yerden devam edilir
```

### Sprint Sonunda (Review + Retro)

```text
Review (Ajan Otonom Kontrolü):
  □ Tüm testler geçiyor mu? (Ajan terminalde dotnet test çalıştırır)
  □ Coverage hedefi tuttu mu?
  □ Security scan temiz mi?
  □ Staging'de test edildi mi?
  □ CHANGELOG güncellendi mi?
  □ README güncellendi mi?
  □ Proje GEMINI.md güncellendi mi?

Retro (kısa):
  + Ne iyi gitti?
  - Ne kötü gitti?
  → Bir sonraki sprint'te ne değişecek?
```

### DONE Tanımı (Definition of Done)

Bir görev ancak tüm bu kriterler sağlandığında "bitti" sayılır:

```text
TIER 1:
  □ Kod yazıldı
  □ Unit test yazıldı ve geçiyor
  □ Terminalde dotnet build hatasız tamamlandı
  □ Code review yapıldı (Gemini Otomatik)
  □ main branch'e merge edildi

TIER 2:
  □ TIER 1 kriterleri +
  □ Integration test geçiyor
  □ Coverage hedefi (%80) sağlandı
  □ Staging'e deploy edildi
  □ Staging'de manuel onay alındı

TIER 3:
  □ TIER 2 kriterleri +
  □ E2E test geçiyor
  □ Performance kriteri sağlandı
  □ Security scan temiz
  □ API dokümantasyonu güncellendi
  □ Müşteri onayı alındı (gerekiyorsa)
```

---

## PERFORMANCE BASELINE

Bir özellik bu değerleri sağlamıyorsa "bitti" sayılmaz.

### Minimum Kabul Kriterleri

```text
API Response Time:
  Okuma (GET)  : p95 < 300ms
  Yazma (POST) : p95 < 500ms
  Rapor/Liste  : p95 < 1000ms

Sayfa Yükleme (Blazor):
  İlk yükleme  : < 3 saniye
  Sonraki      : < 1 saniye

DB Sorgu Süresi:
  Basit sorgu  : < 50ms
  Join'li sorgu: < 200ms
  Rapor sorgusu: < 1000ms

Eş Zamanlı Kullanıcı:
  TIER 1: 10 kullanıcı sorunsuz
  TIER 2: 100 kullanıcı sorunsuz
  TIER 3: 1000 kullanıcı sorunsuz
```

### N+1 Sorgu Kontrolü (N+1 Problemi)

```text
Her yeni endpoint yazıldığında Gemini ajan otonomisi devreye girer:
  1. EF Core query log'unu inceler
  2. Tek istekte kaç sorgu gidiyor?
  3. 1'den fazla → Include() veya Dapper ile çözer

EF Core log aktifleştirme (dev):
  optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
               .EnableSensitiveDataLogging();
```

### Index Kuralları

```text
✓ Foreign key kolonlarına index zorunlu
✓ Sık filtrelenen kolonlara index (IsDeleted, Status, TenantId)
✓ Composite index: en seçici kolon solda
✗ Her kolona index koyma (yazma performansı düşer)
✗ Index olmadan full-table scan kabul etme (TIER 2+)
```

---

## PR & CODE REVIEW AKIŞI

### PR Açmadan Önce Kontrol Listesi

```text
□ Ajan terminalde dotnet build hatasız tamamladı
□ Tüm testler geçiyor (dotnet test)
□ Coverage düşmedi
□ Hardcoded secret yok (Ajan grep ile kontrol eder)
□ TODO/FIXME bırakılmadı (geçici kod yok)
□ Migration varsa → down migration da yazıldı
□ appsettings değişikliği varsa → şablon güncellendi
□ API değişikliği varsa → Scalar docs güncellendi
□ Commit mesajı Conventional format'a uygun
```

### Gemini Otomatik Code Review Kontrol Noktaları

```text
Güvenlik:
  - SQL injection riski var mı? (parametresiz sorgu)
  - XSS açığı var mı?
  - Auth kontrolü atlanmış mı?
  - Hassas veri log'a yazılıyor mu?

Kalite:
  - SOLID ihlali var mı?
  - async/await doğru kullanıldı mı?
  - Exception handling doğru mu? (throw ex yasak)
  - Magic number var mı?

Performance:
  - N+1 sorgu riski var mı?
  - Gereksiz veri çekiliyor mu? (Select * benzeri)
  - Cache kullanılabilecek yer var mı?

Test:
  - Edge case'ler test edildi mi?
  - Negatif senaryolar var mı?
  - Mock doğru kullanıldı mı?
```

### Merge Stratejisi

```text
TIER 0-1: Direkt main'e merge (squash commit)
TIER 2+ : feature → develop → main (Pull Request zorunlu)
          Squash and merge (tek temiz commit)
          Delete branch after merge
```

---

## DEPENDENCY GÜVENLİK YÖNETİMİ

### Güvenlik Açığı Kontrolü

```text
Manuel veya Ajan Otonom kontrolü (her sprint):
  dotnet list package --vulnerable

Otomatik kontrol (TIER 2+):
  GitHub Dependabot → .github/dependabot.yml
  Haftalık otomatik PR açar, güvenlik açıklarını bildirir

CI/CD'de zorunlu (TIER 2+):
  dotnet list package --vulnerable --include-transitive
  Kritik açık varsa → pipeline durdur, deploy etme
```

### Dependabot Yapılandırması (.github/dependabot.yml)

```yaml
version: 2
updates:
  - package-ecosystem: nuget
    directory: "/"
    schedule:
      interval: weekly
    open-pull-requests-limit: 5
```

### Güncelleme Stratejisi

```text
TIER 1: Manuel, ayda bir kontrol
TIER 2: Dependabot otomatik PR + haftalık review
TIER 3: Dependabot + CI security gate + zorunlu güncelleme SLA
         Kritik açık → 24 saat içinde güncelleme zorunlu
```

---

*Global GEMINI.md v8.1 ULTIMATE*
*Kural: Varsayılan ücretsiz · Ücretli sadece talep üzerine*
*Her sprint sonunda gözden geçir ve güncelle.*

---

## BAKIM MODU (Proje Teslim Sonrası)

Teslim edilmiş veya eski bir proje açıldığında Gemini Ajanı bu adımları izler.
Proje ne kadar önce bırakılmış olursa olsun Gemini sanki hiç ayrılmamış gibi devam eder.

```text
1. Proje GEMINI.md oku (bağlamı yükle)
2. Dependency audit çalıştır
   Ajan terminalde: dotnet list package --vulnerable --include-transitive
3. Mevcut testleri çalıştır — hepsi geçmeli
4. Geçmeyen test varsa → önce onu düzelt, sonra göreve geç
5. Kullanıcının görevine geç
```

### Bakım Görev Tipleri

```text
TİP A — Hata Düzeltme (Bug Fix)
  → /hotfix skill çalışır
  → Hedef: 2 saat içinde production

TİP B — Yeni Özellik / Güncelleme
  → Mevcut kodu analiz et (/solid-review)
  → Yeni audit: ek paket gerekiyor mu?
  → Özelliği ekle → test → deploy

TİP C — Güvenlik Güncellemesi
  → Kritik açık: 24 saat içinde güncelle
  → Major açık: 1 hafta içinde
  → dotnet add package [paket] --version [yeni]

TİP D — TIER Yükseltme
  → /tier-upgrade çalışır
  → Kademeli sprint planı oluşturulur
```

---

## MULTI-AGENT ÇALIŞMA KURALLARI

Büyük projelerde birden fazla Gemini Agent paralel çalışabilir.
Çakışma ve veri kaybı riskini önlemek için bu kurallar zorunludur.

### Agent Sayısı (TIER Bazlı)

```text
TIER 0-1 : 1 agent (koordinasyon maliyeti gereksiz)
TIER 2   : 2-3 agent
TIER 3   : 3-5 agent
TIER 4   : 5+ agent (her mikroservis ayrı)
```

### Klasör Sınırları — Her Agent Kendi Bölgesinde Çalışır

```text
Agent 1 — Backend Agent
  Sorumluluk : /src/API + /src/Application + /src/Domain
  Görev      : Endpoint, servis, iş kuralı, validation

Agent 2 — Frontend Agent
  Sorumluluk : /src/Web (Blazor bileşenleri, sayfalar)
  Görev      : UI, layout, form, tablo

Agent 3 — Test Agent
  Sorumluluk : /tests (tüm test projeleri)
  Görev      : Unit, integration, E2E test yazımı

Agent 4 — DevOps Agent
  Sorumluluk : /deploy + /.github/workflows + Dockerfile
  Görev      : CI/CD, migration script, container
```

### Koordinatör Agent Kuralı

```text
Paylaşılan dosyalar sadece Koordinatör Agent tarafından değiştirilir:
  - Program.cs
  - DbContext
  - appsettings.json şablonu
  - Global middleware
  - Shared/Common katmanı

Diğer agentlar bu dosyalara dokunmaz.
İhtiyaç varsa Koordinatör'e bildirir.
```

### Agent Başlatma (Terminal)

```bash
# Paralel agent başlatma (ayrı terminal pencereleri)
gemini --agent backend  "Order modülü: CRUD + validation + testler"
gemini --agent frontend "Order listesi Blazor sayfası + MudBlazor DataGrid"
gemini --agent test     "Order modülü unit + integration testleri, %80 coverage"
gemini --agent devops   "Order migration + CI/CD güncelleme"
```

### Agent Tamamlama Protokolü

```text
Her agent görevi bitince:
1. Kendi klasöründe testleri çalıştır (dotnet test)
2. Build hatasız tamamlandı mı? (dotnet build)
3. Koordinatör Agent'a bildir: "Backend tamamlandı, merge hazır"
4. Koordinatör entegrasyon testini çalıştırır
5. Hepsi geçince → tek PR ile merge
```

---

## UZAKTAN YÖNETİM SİSTEMİ — 4 AŞAMA

### Aşama 1 — Temel Sistem (Şimdi Aktif)
```text
Durum  : Tamamlandı
Ne var : Global GEMINI.md + TIER sistemi + Audit +
         Proje GEMINI.md + Sprint döngüsü + Bakım modu
Kullanım: Gemini'yi manuel başlatıyorsun,
          sistem kendi protokolünü takip ediyor
```

### Aşama 2 — GitHub Actions → Telegram Bildirim
```text
Durum  : Kurulum gerekiyor (1-2 gün)
Ne var : Deploy/test sonucu Telegram'a mesaj geliyor
Örnek  : "✅ CRM v1.2.0 production'a deploy edildi"
         "❌ Test başarısız: OrderService.GetAll (3 hata)"
Maliyet: Ücretsiz
```

### Aşama 3 — Telegram Bot → Gemini API Köprüsü
```text
Durum  : Kurulum gerekiyor (1-2 hafta)
Ne var : Telegram'dan komut gönderiyorsun,
         Gemini API görevi çalıştırıyor
Örnek  : "CRM projesine Excel export ekle"
         → Gemini kodu yazar → GitHub'a push eder
         → Actions çalışır → Telegram'a sonuç gelir
Maliyet: VPS (~€4/ay) + Gemini API kullanım ücreti
```

### Aşama 4 — Tam Otonom Pipeline
```text
Durum  : İleri seviye (orta-uzun vadeli)
Ne var : Sen sadece onay veriyorsun,
         tüm geliştirme + test + deploy otomatik
Örnek  : "Ekle, test et, yayınla" → sistem halleder
         Kritik kararlar için senden onay ister
Maliyet: VPS + Gemini API + monitoring
```

---

## AŞAMA 2 KURULUM REHBERİ
### GitHub Actions → Telegram Bildirim

Bu kurulum tamamlandığında her deploy/test sonucu Telegram'a gelir.

#### Adım 1 — Telegram Bot Oluştur

```text
1. Telegram'da @BotFather'a mesaj at
2. /newbot yaz
3. Bot adını ver (örn: DevOpsBot)
4. Kullanıcı adı ver (örn: sirket_devops_bot)
5. BotFather sana TOKEN verir → kaydet
   Örn: 7432819056:AAFx9K2...

6. Kendi Telegram'ında bota /start mesajı at
7. https://api.telegram.org/bot[TOKEN]/getUpdates adresine git
8. "chat":{"id": 123456789} → bu senin CHAT_ID'in → kaydet
```

#### Adım 2 — GitHub Secrets'a Ekle

```text
GitHub repo → Settings → Secrets and variables → Actions → New secret

TELEGRAM_BOT_TOKEN = [BotFather'dan aldığın token]
TELEGRAM_CHAT_ID   = [kendi chat id'in]
```

#### Adım 3 — GitHub Actions Workflow

```yaml
# .github/workflows/notify.yml
name: Deploy & Notify

on:
  push:
    branches: [main]

jobs:
  build-test-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: "10.0.x"

      - name: Build
        run: dotnet build --configuration Release

      - name: Test
        run: dotnet test --no-build --verbosity normal

      - name: Deploy (FTP veya SSH)
        run: |
          # deploy komutun buraya

      - name: Telegram — Başarı Bildirimi
        if: success()
        run: |
          curl -s -X POST "https://api.telegram.org/bot${{ secrets.TELEGRAM_BOT_TOKEN }}/sendMessage" \
            -d chat_id="${{ secrets.TELEGRAM_CHAT_ID }}" \
            -d text="✅ *${{ github.repository }}*
          Branch: ${{ github.ref_name }}
          Commit: ${{ github.event.head_commit.message }}
          Deploy başarılı 🚀" \
            -d parse_mode=Markdown

      - name: Telegram — Hata Bildirimi
        if: failure()
        run: |
          curl -s -X POST "https://api.telegram.org/bot${{ secrets.TELEGRAM_BOT_TOKEN }}/sendMessage" \
            -d chat_id="${{ secrets.TELEGRAM_CHAT_ID }}" \
            -d text="❌ *${{ github.repository }}*
          Branch: ${{ github.ref_name }}
          HATA: Build veya test başarısız!
          Commit: ${{ github.event.head_commit.message }}" \
            -d parse_mode=Markdown
```

#### Adım 4 — Test Et

```text
1. Herhangi bir dosyayı değiştir
2. git commit + git push
3. GitHub Actions'ta workflow çalışır
4. Telegram'ına mesaj gelir ✅
```

---

## AŞAMA 3 KURULUM REHBERİ
### Telegram Bot → Gemini API Köprüsü

Bu kurulum tamamlandığında Telegram'dan komut gönderip proje yönetebilirsin.

#### Mimari

```text
Telegram Mesajı
    ↓
VPS'teki Python Bot (7/24 çalışır)
    ↓
Gemini API (gemini-3.0-pro)
    ↓
GitHub API (kod push)
    ↓
GitHub Actions (test + deploy)
    ↓
Telegram Bildirimi (sonuç)
```

#### VPS Kurulumu (Hetzner CX11 — €4/ay)

```bash
# Ubuntu 24.04 VPS'te
apt update && apt install -y python3 python3-pip git

pip3 install python-telegram-bot google-genai gitpython

mkdir /opt/devbot && cd /opt/devbot
```

#### Bot Kodu (bot.py)

```python
import asyncio
from google import genai
from google.genai import types
from telegram import Update
from telegram.ext import Application, MessageHandler, filters, ContextTypes

TELEGRAM_TOKEN = "TELEGRAM_BOT_TOKEN_BURAYA"
ALLOWED_USER_ID = 123456789  # Sadece senin ID'in
GEMINI_API_KEY = "GEMINI_API_KEY_BURAYA"

client = genai.Client(api_key=GEMINI_API_KEY)

# Aktif proje bağlamı
project_context = {}

async def handle_message(update: Update, context: ContextTypes.DEFAULT_TYPE):
    # Sadece yetkili kullanıcı
    if update.effective_user.id != ALLOWED_USER_ID:
        return

    user_msg = update.message.text
    await update.message.reply_text("⏳ Gemini Çalışıyor...")

    # Proje bağlamını oluştur
    system_prompt = f"""
    Sen bir yazılım geliştirme asistanısın. Ajan modunda çalışıyorsun.
    Mevcut proje bağlamı: {project_context}
    Global GEMINI.md kurallarını uyguluyorsun.
    Görev: {user_msg}
    
    Yapacaklarını adım adım açıkla ve uygula.
    """

    response = client.models.generate_content(
        model='gemini-3.0-pro',
        contents=user_msg,
        config=types.GenerateContentConfig(
            system_instruction=system_prompt,
        ),
    )

    result = response.text
    
    # Uzunsa böl
    if len(result) > 4000:
        for i in range(0, len(result), 4000):
            await update.message.reply_text(result[i:i+4000])
    else:
        await update.message.reply_text(result)

def main():
    app = Application.builder().token(TELEGRAM_TOKEN).build()
    app.add_handler(MessageHandler(filters.TEXT, handle_message))
    print("Gemini Bot çalışıyor...")
    app.run_polling()

if __name__ == "__main__":
    main()
```

#### Servis Olarak Çalıştır (7/24 aktif)

```bash
# /etc/systemd/system/devbot.service
[Unit]
Description=DevOps Telegram Bot (Gemini)
After=network.target

[Service]
Type=simple
User=root
WorkingDirectory=/opt/devbot
ExecStart=/usr/bin/python3 /opt/devbot/bot.py
Restart=always
RestartSec=10

[Install]
WantedBy=multi-user.target

# Aktif et
systemctl enable devbot
systemctl start devbot
systemctl status devbot
```

#### Kullanım Örnekleri

```text
Sen yazıyorsun:
  "CRM projesine Excel export özelliği ekle"

Bot cevaplıyor:
  "⏳ Gemini Çalışıyor...
   ✅ ClosedXML paketi eklendi
   ✅ ExportService yazıldı
   ✅ /api/orders/export endpoint'i eklendi
   ✅ Test yazıldı ve geçti (dotnet test)
   ✅ GitHub'a push edildi
   🚀 Deploy başladı, 3 dakika içinde canlıda"

---

Sen yazıyorsun:
  "Bugün hangi görevler bitti?"

Bot cevaplıyor:
  "📋 Bugün tamamlananlar:
   ✅ Excel export özelliği
   ✅ 3 bug fix (issue #12, #15, #18)
   ⏳ Devam eden: Dashboard widget"
```

#### Güvenlik Kuralları

```text
✓ Sadece kendi Telegram ID'in kabul edilir
✓ Bot token'ı asla kod içine yazma → env variable
✓ Gemini API key → env variable
✓ VPS'e sadece SSH key ile giriş
✓ Production deploy için ek onay adımı
  "production'a deploy et" → "Emin misin? (evet/hayır)"
✗ Bota admin server erişimi verme
✗ Token'ı kimseyle paylaşma
```

---

## .NET VERSİYON KURALI

```text
Her zaman en son kararlı .NET sürümü kullanılır.
Şu an: .NET 10.0 (LTS)

Yeni proje oluştururken terminalde ajan komutu:
  dotnet new blazorserver -n ProjeAdı --framework net10.0

Mevcut proje güncelleme:
  .csproj içinde <TargetFramework>net10.0</TargetFramework>

Sonraki LTS çıkınca (.NET 12 vb.) bu kural güncellenir.
```

---

## MODEL KULLANIMI

```text
Gemini CLI (Terminal Geliştirme) → Gemini 3 Pro (Ajan Modu) ← her zaman bu
Gemini Web (Sohbet/Planlama)      → Gemini 3 Pro               ← planlama, soru

Gemini Code başlatma komutu:
  gemini /model "Gemini 3 Pro"
```

---

## PUBLISH & DEPLOY (FTP İÇİN)

Her projede kullanılacak publish komutu (Ajan terminalde çalıştırır):

```bash
# Release build + publish
dotnet publish -c Release -o ./publish

# Sonuç: [ProjeKlasörü]\publish\ dizinine çıkar
# Bu dizini FTP ile sunucuna at
```

Gemini Code Ajanına söylemek için:
```text
Projeyi publish et, ./publish klasörüne çıkar.
FTP ile manuel transfer edeceğim.
```

---

## TOKEN OPTİMİZASYON KURALLARI

Gemini Ajanının token kullanımını minimize etmek için:

### Oturum Başı
```text
✓ Proje GEMINI.md'yi bir kez oku, tekrar okuma
✓ Sadece ilgili dosyaları oku (tüm projeyi tarama)
✓ Görev net verilirse analiz adımını atla
```

### Görev Verme
```text
✓ Tek seferde net ve tam görev ver
  KÖTÜ : "bir şeyler ekle"
  İYİ  : "Order modülüne soft delete ekle, EF Core
          global query filter kullan, test yaz"

✓ Büyük görevi parçala
  KÖTÜ : "tüm projeyi yeniden yaz"
  İYİ  : "önce servis katmanını güncelle"
         sonra "şimdi UI katmanını güncelle"

✓ Hata düzeltmede sadece hata metnini ver (Ajan terminalden okur)
  KÖTÜ : "bir hata var, ne olduğunu bul"
  İYİ  : "InvalidOperationException: ... aldım. Düzelt."
```

### Dosya Okuma
```text
✓ "Sadece [DosyaAdı] dosyasına bak" de
✓ "Tüm projeyi tara" deme → gereksiz token
✓ Büyük dosyalarda "sadece [metod adı] metoduna bak" de
```

### Onay Ekranları
```text
✓ "Yes, and don't ask again" seçeneklerini kullan
  → Tekrar tekrar onay = tekrar tekrar token
✓ İzinleri bir kez ver, kalıcı hale getir
```

### Bağlam Yönetimi
```text
✓ Uzun oturumlarda ara ara "özet çıkar" de
✓ Yeni oturumda "proje GEMINI.md oku, devam et" de
✓ Alakasız sohbeti terminalde yapma
  → Planlama = Web | Kodlama = Ajan Terminali
```

---

## UI TEMA SİSTEMİ

### Varsayılan Tema (Her Blazor Projesinde Otomatik)

Blazor projesi oluşturulduğunda bu tema otomatik uygulanır.
Kullanıcı başka tema belirtmemişse Modern Minimal kullanılır.

```text
Varsayılan: Modern Minimal
- Kütüphane : MudBlazor (her zaman)
- Arka plan : #F8FAFC (açık gri)
- Primary   : #6366F1 (indigo)
- Secondary : #06B6D4 (cyan)
- Yazı      : #1E293B (koyu gri)
- Kartlar   : beyaz, hafif gölge, rounded
- Layout    : sol sidebar + üst header
- Responsive: mobil + masaüstü zorunlu
```

### Hazır Tema Komutları

Terminale kısa komut yaz, Gemini ajan ilgili temayı uygular ve terminalde `dotnet build` yapar:

```text
/tema minimal    → Modern & Minimal (beyaz, indigo, temiz)
/tema dark       → Dark Mode (koyu arka plan, mor/cyan vurgu)
/tema corporate  → Kurumsal (lacivert, gri, profesyonel)
/tema colorful   → Renkli & Canlı (gradient, bold renkler)
/tema custom     → Özel renk ister (primary ve secondary sor)
```

### Tema Detayları

```text
/tema minimal
  Arka plan : #F8FAFC
  Primary   : #6366F1
  Secondary : #06B6D4
  Kart      : beyaz + shadow-sm
  Buton     : rounded-lg, flat
  Font      : Inter / system-ui

/tema dark
  Arka plan : #0F172A
  Surface   : #1E293B
  Primary   : #818CF8
  Secondary : #22D3EE
  Yazı      : #F1F5F9
  Kart      : #1E293B + border

/tema corporate
  Arka plan : #F1F5F9
  Primary   : #1E3A5F
  Secondary : #2D6DB5
  Accent    : #E07B00
  Kart      : beyaz + border-left vurgu
  Font      : Arial / sans-serif

/tema colorful
  Arka plan : #FAFAFA
  Primary   : #7C3AED (mor)
  Secondary : #F59E0B (amber)
  Gradient  : mor → cyan header
  Kart      : beyaz + renkli top border
```

### Tema Uygulama Komutu (Hazır Prompt)

```text
[/tema minimal] komutunu çalıştır:
TodoApp projesine Modern Minimal tema uygula.
MudThemeProvider ile global tema tanımla.
Tüm sayfalar responsive olsun.
Terminalde Build al, 0 hata olsun.
```

---

## TEMA SKILL'LERİ (Kısa Komutlar)

Terminale sadece aşağıdaki komutu yaz, Gemini Ajanı gerisini halleder.

### /tema minimal
```text
Modern & Minimal tema uygula.
Primary #6366F1, Secondary #06B6D4, Background #F8FAFC.
MudThemeProvider global tanımla, tüm sayfalar responsive,
sol sidebar + üst header layout, terminalde build al 0 hata olsun.
```

### /tema dark
```text
Dark Mode tema uygula.
Background #0F172A, Surface #1E293B, Primary #818CF8, Secondary #22D3EE, Yazı #F1F5F9.
MudThemeProvider global tanımla, tüm sayfalar responsive,
sol sidebar + üst header layout, terminalde build al 0 hata olsun.
```

### /tema corporate
```text
Kurumsal tema uygula.
Primary #1E3A5F, Secondary #2D6DB5, Accent #E07B00, Background #F1F5F9.
MudThemeProvider global tanımla, tüm sayfalar responsive,
sol sidebar + üst header layout, terminalde build al 0 hata olsun.
```

### /tema colorful
```text
Renkli & Canlı tema uygula.
Primary #7C3AED, Secondary #F59E0B, gradient header mor→cyan.
MudThemeProvider global tanımla, tüm sayfalar responsive,
sol sidebar + üst header layout, terminalde build al 0 hata olsun.
```

### /tema custom
```text
Kullanıcıdan primary ve secondary renk kodlarını sor,
sonra o renklerle MudThemeProvider global tanımla,
tüm sayfalar responsive, terminalde build al 0 hata olsun.
```

### /publish
```text
Projeyi release modda publish et:
dotnet publish -c Release -o ./publish
Hata varsa terminal üzerinden düzelt, 0 hata ile tamamla.
FTP transferi için ./publish klasörü hazır.
```

### /bakim
```text
Bakım modu başlat:
Proje GEMINI.md oku, terminalde dependency audit yap,
mevcut testleri çalıştır (dotnet test), özet rapor sun.
```

### /tasarim-iyilestir
```text
Tüm sayfalarda tasarım sorunlarını tespit et ve düzelt:
kaymalar, hizalama bozuklukları, mobil uyumsuzluklar.
MudBlazor responsive grid kullan, terminalde build al 0 hata olsun.
```