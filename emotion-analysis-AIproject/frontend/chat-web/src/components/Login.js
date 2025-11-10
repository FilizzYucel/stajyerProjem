import React, { useState } from 'react';

// CSS'i (Stilleri) doğrudan bu dosyada yazacağız
// Bu, Login.js'in kendi stilini içinde taşımasını sağlar
const loginStyles = {
    container: {
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        padding: '20px',
        backgroundColor: 'white',
        borderRadius: '8px',
        boxShadow: '0 4px 12px rgba(0, 0, 0, 0.1)',
        width: '300px'
    },
    input: {
        padding: '10px 15px',
        border: '1px solid #ddd',
        borderRadius: '18px',
        marginBottom: '10px',
        width: '80%',
        fontSize: '1em'
    },
    button: {
        border: 'none',
        backgroundColor: '#007bff',
        color: 'white',
        padding: '10px 15px',
        borderRadius: '18px',
        cursor: 'pointer',
        fontSize: '1em'
    }
};

// Bu bileşen (Login), giriş yapıldığında ana dosyamıza (App.js)
// haber vermek için 'onLogin' adında bir fonksiyon alacak.
function Login({ onLogin }) {
    const [nickname, setNickname] = useState("");

    const handleLogin = () => {
        // Sadece rumuz girildiyse 'onLogin' fonksiyonunu çalıştır
        if (nickname.trim()) {
            onLogin(nickname); // App.js'e "İşte rumuz bu!" diye haber ver
        }
    };

    return (
        <div style={loginStyles.container}>
            <h3>Sohbete Katıl</h3>
            <input
                type="text"
                placeholder="Rumuzunuzu girin..."
                style={loginStyles.input}
                value={nickname}
                onChange={(e) => setNickname(e.target.value)}
                // Enter tuşuna basınca da giriş yapmayı sağlar
                onKeyPress={(e) => e.key === 'Enter' && handleLogin()}
            />
            <button style={loginStyles.button} onClick={handleLogin}>
                Katıl
            </button>
        </div>
    );
}

export default Login;