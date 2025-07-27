# SurveySystem - Anket Sistemi API

Bu proje, bir Anket Sistemi API'sidir. Veri tabanı olarak **MySQL** kullanılmaktadır.

## 📊 Veritabanı Tabloları

### 1. Users

| Alan Adı   | Açıklama |
|------------|----------|
| Id         | Kullanıcı ID'si |
| Email      | Kullanıcının e-posta adresi |
| Username   | Kullanıcı adı |
| Password   | Şifre (hashli) |
| CreatedAt  | Oluşturulma tarihi |

> Ek olarak kullanıcıya ait daha fazla bilgi tutulabilir. Örneğin eğitim durumu, yaş, cinsiyet gibi.  
> Ayrıca `IsActive` adında bir sütunla kullanıcı aktif/pasif durumu yönetilebilir (örneğin hesap silme işlemleri için).

---

### 2. Surveys

| Alan Adı    | Açıklama |
|-------------|----------|
| Id          | Anket ID'si |
| Title       | Anket başlığı |
| Description | Açıklama |
| UserId      | Anketi oluşturan kullanıcı ID'si |
| CreatedAt   | Oluşturulma tarihi |

> `Surveys.UserId` ile `Users.Id` arasında **bire çok ilişki** bulunmaktadır.

---

### 3. SurveyOptions

| Alan Adı    | Açıklama |
|-------------|----------|
| Id          | Seçenek ID'si |
| SurveyId    | Bağlı olduğu anketin ID'si |
| OptionText  | Seçeneğin metni |

> Bir anketin birden fazla seçeneği olabilir.  
> `SurveyOptions.SurveyId` ile `Surveys.Id` arasında **bire çok ilişki** bulunmaktadır.

---

### 4. Votes

| Alan Adı        | Açıklama |
|------------------|----------|
| Id               | Oy ID'si |
| UserId           | Oyu veren kullanıcının ID'si |
| SurveyId         | Oylanan anketin ID'si |
| ServerOptionId   | Seçilen anket seçeneğinin ID'si |
| VotedAt          | Oy verme zamanı |

> Bu tabloda ankete verilen cevaplar tutulur.  
> `UserId` ➝ `Users`, `SurveyId` ➝ `Surveys`, `ServerOptionId` ➝ `SurveyOptions` tabloları ile ilişkilidir.

---

## 🔗 İlişkiler Özeti

- **Users ↔ Surveys**: 1:N (bir kullanıcı birden fazla anket oluşturabilir)
- **Surveys ↔ SurveyOptions**: 1:N (bir anketin birden fazla seçeneği olabilir)
- **Users ↔ Votes**: 1:N (bir kullanıcı birden fazla oy kullanabilir)
- **SurveyOptions ↔ Votes**: 1:N (bir seçenek birden fazla oy alabilir)

---

> Bu yapı, bir anket sisteminin temel işleyişini karşılayacak şekilde esnek ve ilişkilidir. Geliştirmeye açıktır.

# Swager çıktıları aşağıdaki gibidir:
<img width="1448" height="663" alt="1- Kayıt olma" src="https://github.com/user-attachments/assets/f18b7965-3458-4059-92af-cbeb1a846c11" />
<img width="1411" height="675" alt="2- Kayıt Başarılı" src="https://github.com/user-attachments/assets/2430c635-ff01-4950-ba0d-504772656fe5" />
<img width="658" height="304" alt="3- Mysql users tablosu" src="https://github.com/user-attachments/assets/28615fb5-e55d-408e-a5f7-63012eb7d453" />
<img width="1410" height="599" alt="4- Kullanıcı giriş" src="https://github.com/user-attachments/assets/f97694ee-ec1c-4591-a5f7-67a1ff9dcf66" />
<img width="1400" height="738" alt="5- Giriş başarılı response ve JWT token" src="https://github.com/user-attachments/assets/65813643-1221-4a33-a437-af0739678fb4" />
<img width="639" height="312" alt="55- jwt token giriş" src="https://github.com/user-attachments/assets/b80f7107-3340-40ea-b085-3f5a4eea3e9a" />
<img width="1422" height="582" alt="6- Anket oluşturma" src="https://github.com/user-attachments/assets/47f4e1b0-f95d-4a07-bfcf-67f3e5bba4dc" />
<img width="1405" height="862" alt="7- Anket oluşturma response" src="https://github.com/user-attachments/assets/cff79b56-0981-4355-b8ef-33742004a51f" />
<img width="961" height="275" alt="8- Mysql surveys tablosu" src="https://github.com/user-attachments/assets/2919088b-a0b2-4bbd-9982-1504bcb1c43c" />
<img width="488" height="335" alt="9- Mysql surveyoption tablosu" src="https://github.com/user-attachments/assets/21dee18b-2450-4fb3-a206-ace3c79ff6d1" />
<img width="1413" height="895" alt="10- Id'ye göre anket getirme" src="https://github.com/user-attachments/assets/bd9bf19c-aa3e-4a8b-99e5-e98a938b8c0b" />
<img width="1412" height="848" alt="11- Anketlerim" src="https://github.com/user-attachments/assets/ac47e180-0f0a-42a6-abc2-f72e8f9bfab5" />
<img width="1424" height="343" alt="12- Anket sonuçları request" src="https://github.com/user-attachments/assets/006428de-f960-4246-8fe5-b156d7ea66c4" />
<img width="1392" height="459" alt="13- Anket sonuçları response" src="https://github.com/user-attachments/assets/8f4b2684-b676-44c7-a471-75b413a224f2" />
<img width="1423" height="650" alt="14- Anket oylama" src="https://github.com/user-attachments/assets/c153854f-1c6b-4eb8-8d32-d7a10f0076d6" />
<img width="1403" height="707" alt="15- sonuç" src="https://github.com/user-attachments/assets/8dacd2dc-0f91-4c16-838d-8f1443cb3d78" />
<img width="1398" height="759" alt="16- sonuç 2" src="https://github.com/user-attachments/assets/f49ceb7f-b6f8-4cac-8470-5402134e83ba" />


