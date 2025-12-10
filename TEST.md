# اختبار موقع LaLa Store

## الخطوات:

1. **تأكد أن التطبيق يعمل:**
   - افتح PowerShell أو Terminal
   - اذهب إلى: `cd C:\Users\LaLa-Store\security-platform\LaLaStore`
   - شغل: `dotnet run`
   - انتظر حتى ترى رسالة: "Now listening on: http://localhost:5051"

2. **افتح المتصفح:**
   - افتح Chrome أو Edge
   - اكتب في شريط العنوان: `http://localhost:5051`
   - أو: `http://127.0.0.1:5051`

3. **إذا لم يعمل:**
   - تأكد أن لا يوجد برنامج آخر يستخدم المنفذ 5051
   - جرب المنفذ 7292 للـ HTTPS
   - تحقق من رسائل الخطأ في Terminal

## روابط الموقع:
- الصفحة الرئيسية: http://localhost:5051
- المنتجات: http://localhost:5051/Products
- السلة: http://localhost:5051/Cart

