import ssl
import websocket
import json

from OmegaExpansion import pwmExp


ws = None
freq=50
dutycycle=3   

def encode_json(obj):    
    return json.dumps(obj) + chr(0x1E)

def ws_on_message(ws, message: str):
    ignore_list = ['{"type":6}', '{}']
    for msg in message.split(chr(0x1E)):
        if msg and msg not in ignore_list:
            print(f"From server: {msg}")
            jmsg = json.loads(msg)
            print (jmsg["arguments"][1])
            # orientation read from signar message
            lrangle = float(jmsg["arguments"][1])
            #converting the orientation to a PWM signal
            x = (lrangle + 90.0) / 180.0            
            dutycycle = 3.0 + x * (13.5-3.0)      
            pwmExp.setupDriver(0,dutycycle,0)      

def ws_on_error(ws, error):
    print(error)

def ws_on_close(ws):
    print("### Disconnected from SignalR Server ###")

def ws_on_open(ws):
    print("### Connected to SignalR Server via WebSocket ###")
    
    # Do a handshake request
    print("### Performing handshake request ###")
    ws.send(encode_json({
        "protocol": "json",
        "version": 1
    }))

    # Handshake completed
    print("### Handshake request completed ###")    

if __name__ == "__main__":
    pwmExp.driverInit()    
    pwmExp.setFrequency(freq)    
    websocket.enableTrace(True)

    ## Use wss as protocol for your url. for example, wss://localshot:44321/messagehub

    ws = websocket.WebSocketApp("[YOUR SIGNALR URL HERE]",
                              on_message = ws_on_message,
                              on_error = ws_on_error,
                              on_close = ws_on_close)
    ws.on_open = ws_on_open
    ws.run_forever(sslopt={"cert_reqs": ssl.CERT_NONE})
