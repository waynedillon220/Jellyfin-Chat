(function () {
    if (document.getElementById("jf-chat-bubble")) return;

    // 💬 Chat bubble
    const bubble = document.createElement("div");
    bubble.id = "jf-chat-bubble";
    bubble.innerText = "💬";

    Object.assign(bubble.style, {
        position: "fixed",
        bottom: "20px",
        right: "20px",
        width: "50px",
        height: "50px",
        background: "#222",
        color: "white",
        borderRadius: "50%",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        cursor: "pointer",
        zIndex: 9999
    });

    // 🪟 Chat window
    const chat = document.createElement("div");
    chat.id = "jf-chat-window";

    Object.assign(chat.style, {
        position: "fixed",
        bottom: "80px",
        right: "20px",
        width: "300px",
        height: "400px",
        background: "#111",
        color: "white",
        display: "none",
        flexDirection: "column",
        zIndex: 9999,
        borderRadius: "10px",
        overflow: "hidden"
    });

    chat.innerHTML = `
        <div style="padding:5px;background:#222;">Local Chat</div>
        <div id="jf-chat-messages" style="flex:1;overflow:auto;padding:5px;"></div>
        <div id="jf-chat-typing" style="font-size:12px;padding-left:5px;"></div>
        <input id="jf-chat-input" placeholder="Type..." style="border:none;padding:5px;width:100%;" />
    `;

    document.body.appendChild(bubble);
    document.body.appendChild(chat);

    bubble.onclick = () => {
        chat.style.display = chat.style.display === "none" ? "flex" : "none";
    };

    // 🔌 WebSocket
    const ws = new WebSocket(`ws://${location.host}/api/LocalChat/ws`);

    ws.onmessage = (event) => {
        const data = JSON.parse(event.data);
        const messages = document.getElementById("jf-chat-messages");

        if (data.type === "history") {
            data.messages.forEach(addMessage);
        }

        if (data.type === "message") {
            addMessage(data);
        }

        if (data.type === "delete") {
            const el = document.getElementById(data.id);
            if (el) el.innerText = "(deleted message)";
        }

        if (data.type === "typing") {
            document.getElementById("jf-chat-typing").innerText =
                data.user + " is typing...";
        }
    };

    function addMessage(m) {
        const el = document.createElement("div");
        el.id = m.id;

        el.innerHTML = `<b>${m.user || m.username}:</b> ${m.text || m.message}`;

        el.oncontextmenu = (e) => {
            e.preventDefault();
            ws.send(JSON.stringify({ type: "delete", id: m.id }));
        };

        document.getElementById("jf-chat-messages").appendChild(el);
    }

    document.getElementById("jf-chat-input").addEventListener("keydown", (e) => {
        if (e.key === "Enter") {
            ws.send(JSON.stringify({
                type: "message",
                text: e.target.value
            }));
            e.target.value = "";
        } else {
            ws.send(JSON.stringify({ type: "typing" }));
        }
    });

    // 🎬 Hide during playback UI fade
    const observer = new MutationObserver(() => {
        const videoControls = document.querySelector(".videoOsdBottom");

        if (videoControls && videoControls.style.opacity === "0") {
            bubble.style.opacity = "0";
            chat.style.opacity = "0";
        } else {
            bubble.style.opacity = "1";
            chat.style.opacity = "1";
        }
    });

    observer.observe(document.body, {
        attributes: true,
        subtree: true
    });

})();