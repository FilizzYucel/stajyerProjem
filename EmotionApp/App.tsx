import React, { useState, useEffect } from 'react';
import { SafeAreaView, View, Text, TextInput, Button, FlatList, StyleSheet } from 'react-native';
import axios from 'axios';

//const API_URL = "http://localhost:5206/api/chat"; 
const API_URL = "http://10.0.2.2:5206/api/chat";

interface Message {
  id: number;
  text: string;
  nickname: string;
  timestamp: string;
  sentiment: string;
}

export default function App() {
  const [nickname, setNickname] = useState("Filiz");
  const [text, setText] = useState("");
  const [messages, setMessages] = useState<Message[]>([]);

  useEffect(() => {
    const fetchMessages = async () => {
      try {
        const res = await axios.get(`${API_URL}/messages`);
        setMessages(res.data);
      } catch (err) {
        console.error("Mesajlar çekilemedi:", err);
      }
    };
    fetchMessages();
  }, []);

  const sendMessage = async () => {
    if (!text.trim()) return;
    const newMessage = { Nickname: nickname, Text: text };

    try {
      const res = await axios.post(`${API_URL}/send`, newMessage);
      setMessages(prev => [...prev, res.data]);
      setText("");
    } catch (err) {
      console.error("Mesaj gönderilemedi:", err);
    }
  };

  return (
    <SafeAreaView style={styles.container}>
      <FlatList
        data={messages}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <View style={styles.messageBox}>
            <Text style={styles.nickname}>{item.nickname}:</Text>
            <Text style={styles.text}>{item.text}</Text>
            <Text style={styles.sentiment}>Duygu: {item.sentiment}</Text>
          </View>
        )}
      />
      <TextInput
        style={styles.input}
        placeholder="Mesajınızı yazın..."
        value={text}
        onChangeText={setText}
      />
      <Button title="Gönder" onPress={sendMessage} />
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    padding: 10,
  },
  messageBox: {
    backgroundColor: '#eaeaea',
    padding: 8,
    borderRadius: 8,
    marginVertical: 5,
  },
  nickname: {
    fontWeight: 'bold',
  },
  text: {
    fontSize: 16,
  },
  sentiment: {
    fontStyle: 'italic',
    color: '#555',
  },
  input: {
    borderWidth: 1,
    borderColor: '#ccc',
    padding: 8,
    marginVertical: 10,
    borderRadius: 8,
  },
});
