# تعليمات تشغيل موقع LaLa Store

## موقع LaLa Store منفصل تماماً عن موقع Security

### كيفية التشغيل:

1. افتح Terminal أو PowerShell
2. اذهب إلى مجلد المشروع:
```bash
cd C:\Users\LaLa-Store\security-platform\LaLaStore
```

3. شغل المشروع:
```bash
dotnet run
```

4. افتح المتصفح على أحد هذه الروابط:
   - **HTTP**: http://localhost:5051
   - **HTTPS**: https://localhost:7292

### ملاحظات مهمة:

- ✅ المشروع مستقل تماماً عن موقع security
- ✅ يعمل على منافذ مختلفة (5051 و 7292)
- ✅ لا يؤثر على موقع security بأي شكل

### إذا ظهر خطأ "Connection Refused":

1. تأكد أن المشروع يعمل (يجب أن ترى رسالة في Terminal)
2. تأكد أنك تستخدم المنفذ الصحيح (5051 أو 7292)
3. تأكد أن لا يوجد برنامج آخر يستخدم نفس المنفذ

