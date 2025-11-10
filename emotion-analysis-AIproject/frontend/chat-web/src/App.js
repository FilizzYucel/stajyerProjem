import React, { useState } from 'react';
import './App.css'; // Global stiller (sohbet kutusu, mesajlar, arkaplan)
import Login from './components/Login.js'; // Oluşturduğumuz Giriş ekranı
import Chat from './components/Chat.js';   // Oluşturduğumuz Sohbet ekranı 

function App() {
  // Ana 'nickname' (rumuz) durumu burada, App.js'te tutulacak.
  // Başlangıçta boştur, bu da "kullanıcı giriş yapmadı" anlamına gelir.
  const [nickname, setNickname] = useState("");

  // Bu fonksiyonu, Login.js'e bir "prop" (özellik) olarak göndereceğiz.
  // Login.js'teki "Katıl" butonuna basıldığında bu fonksiyon çalışacak.
  const handleLogin = (loggedInNickname) => {
    setNickname(loggedInNickname); // Kullanıcının girdiği rumuzu kaydet
  };

  // Koşullu Gösterim (Conditional Rendering):
  // React'teki en önemli mantıklardan biridir.
  return (
    <div className="App">
      {/* Ternary Operatörü ( ? : ) kullanıyoruz: */}

      {/* EĞER nickname === "" (boş) ise... */}
      {nickname === "" ? (
        // ...O zaman Login bileşenini göster
        <Login onLogin={handleLogin} />
      ) : (
        // ...DEĞİLSE (yani nickname doluysa)...
        // ...O zaman Chat bileşenini göster
        <Chat nickname={nickname} />
      )}
    </div>
  );
}

export default App;