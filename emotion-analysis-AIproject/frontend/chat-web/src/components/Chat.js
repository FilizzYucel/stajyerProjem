import '../App.css'; 
import React, { useState, useEffect } from 'react';
// .NET API'mizle konuşmak için 'axios' kütüphanesini import et
import axios from 'axios'; 

// ---- YENİ BÖLÜM: API ADRESİ ----
// Render üzerinde yayınladığımız .NET API adresi
// (Örnek: https://stajyerprojem-api.onrender.com)
const API_URL = "https://stajyerprojem-api.onrender.com/api/chat";

function Chat({ nickname }) {
  const [messages, setMessages] = useState([]); 
  const [text, setText] = useState(""); 

  // ---- DEĞİŞEN BÖLÜM: useEffect ----
  // Bu 'useEffect', bileşen ilk açıldığında 
  // sahte veri yerine API'den GERÇEK verileri çekmek için 1 kez çalışır.
  useEffect(() => {
    // API'den eski mesajları çekmek için fonksiyon
    const fetchMessages = async () => {
      try {
        // .NET API'mize GET isteği at: http://localhost:5206/api/chat/messages 
        const response = await axios.get(`${API_URL}/messages`);

        // API'den gelen verileri (response.data) 'messages' state'ine kaydet
        setMessages(response.data);
      } catch (error) {
        console.error("Mesajlar çekilirken hata oluştu:", error);
      }
    };

    fetchMessages(); // Fonksiyonu çalıştır
  }, []); // '[]' sayesinde sadece 1 kez çalışır

  // ---- DEĞİŞEN BÖLÜM: handleSendMessage ----
  // Mesaj gönderme fonksiyonu artık API'ye POST isteği atacak
  const handleSendMessage = async () => {
    if (!text.trim()) return; // Boş mesajı engelle

    // 1. API'ye gönderilecek veri paketi (DTO)
    const messageRequest = {
      Nickname: nickname, // Giriş yapan kullanıcının rumuzu 
      Text: text          // Yazılan mesaj 
    };

    try {
      // 2. .NET API'mize POST isteği at: http://localhost:5206/api/chat/send 
      const response = await axios.post(`${API_URL}/send`, messageRequest);

      // 3. API'den dönen cevap (içinde AI analizi olan TAM mesaj)
      const newMessageFromApi = response.data;

      // 4. Yeni mesajı mevcut mesaj listesine ekle
      // (Bu, 'setMessages'in en doğru anlık güncelleme yoludur)
      setMessages(prevMessages => [...prevMessages, newMessageFromApi]);

      setText(""); // Mesaj kutusunu temizle

    } catch (error) {
      console.error("Mesaj gönderilirken hata oluştu:", error);
    }
  };

  return (
    <div className="chat-container">
      <div className="message-list">
        {messages.map((msg) => (
          <div
            key={msg.id}
            className={`message ${msg.nickname === nickname ? 'sent' : 'received'}`}
          >
            {msg.nickname !== nickname && (
              <div className="message-sender">{msg.nickname}</div>
            )}
            {msg.text}
            <div className="message-sentiment">
              {/* Artık gerçek AI sonucunu gösteriyoruz! */}
              Duygu: {msg.sentiment}
            </div>
          </div>
        ))}
      </div>
      <div className="input-area">
        <input
          type="text"
          placeholder="Mesajınızı yazın..."
          value={text}
          onChange={(e) => setText(e.target.value)}
          onKeyPress={(e) => e.key === 'Enter' && handleSendMessage()}
        />
        <button onClick={handleSendMessage}>Gönder</button>
      </div>
    </div>
  );
}

export default Chat;