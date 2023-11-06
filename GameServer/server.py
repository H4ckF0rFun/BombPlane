import http.server
import socketserver
import json
import threading
import time
import os
import game

class MyHttpHandler(http.server.SimpleHTTPRequestHandler):
    hander_map  = {}    
    gameManager = game.GameManager()

    def _init_handler_map_() -> None:
        MyHttpHandler.hander_map['creategame'] = MyHttpHandler.OnCreateGame
        MyHttpHandler.hander_map['confirm'] = MyHttpHandler.OnConfirm
        MyHttpHandler.hander_map['queryinfo'] = MyHttpHandler.OnQueryInfo
        MyHttpHandler.hander_map['joingame'] = MyHttpHandler.OnJoinGame
        MyHttpHandler.hander_map['putplane'] = MyHttpHandler.OnPutPlane
        MyHttpHandler.hander_map['removeplane'] = MyHttpHandler.OnRemovePlane
        MyHttpHandler.hander_map['attack'] = MyHttpHandler.OnAttack
        
    def do_GET(self) -> None:
        if len(self.path) > len('/api/') and self.path[:len('/api/')] == '/api/':
            return self.api()
        return super().do_GET()

    def do_POST(self) -> None:
        if len(self.path) > len('/api/') and self.path[:len('/api/')] == '/api/':
            return self.api()
        return super().do_GET()
    
    def api(self) -> None:
        end = len(self.path)
        api_name = self.path[len('/api/'):end]

        handler = MyHttpHandler.hander_map.get(api_name)
        
        if handler:            
            return handler(self)
        else:
            self.send_error(404,"Not Supported Interface")
            

    
    
    '''
    Creat Game.
    -->
    {
        "nickname" : nickname,
        "password" : password
    }
    
    <--
    {
        "result"  : 0,
        "error"   : "",
        "session" : ""
    }
    '''
    
    def OnAttack(self) ->None:
        length = 0
        try:
            length = int(self.headers['content-length'])
            body = self.rfile.read(length).decode()
            resquest = json.loads(body)
            
            session = resquest['session']
            nickname = resquest['nickname']
            password = resquest['password']
            x = resquest["x"]
            y = resquest["y"]
            
            
            code,err,value = MyHttpHandler.gameManager.attack(session,nickname,password,x,y)
            
            res = json.dumps({
                "result"  : code,
                "error"   : err,
                "value" : value
            })

            print(res)
            self.send_response(200,'OK')
            self.send_header('content-length',len(res))
            self.send_header('Connection','close')
            self.end_headers()
            self.flush_headers()

            self.wfile.write(res.encode('utf-8'))
    
        except:
            self.send_response(500)
            self.end_headers()
            self.flush_headers()
            pass
        
    def OnConfirm(self) ->None:
        length = 0
        try:
            length = int(self.headers['content-length'])
            body = self.rfile.read(length).decode()
            resquest = json.loads(body)
            
            session = resquest['session']
            nickname = resquest['nickname']
            password = resquest['password']
            
            code,err = MyHttpHandler.gameManager.confirm(session,nickname,password)
            
            res = json.dumps({
                "result"  : code,
                "error"   : err
            })

            print(res)
            self.send_response(200,'OK')
            self.send_header('content-length',len(res))
            self.send_header('Connection','close')
            self.end_headers()
            self.flush_headers()

            self.wfile.write(res.encode('utf-8'))
        
        except:
            self.send_response(500)
            self.end_headers()
            self.flush_headers()
            pass
        
    def OnQueryInfo(self) ->None:
        length = 0
        try:
            length = int(self.headers['content-length'])
            body = self.rfile.read(length).decode()
            resquest = json.loads(body)
            
            session = resquest['session']
            nickname = resquest['nickname']
            password = resquest['password']
            
            state,session,player_0,player_1,p1_plane,p2_plane,turn,attack_pos,win,p0_left_time,p1_left_time = MyHttpHandler.gameManager.getInfo(session,nickname,password)

            player_0 = '' if player_0 == None else player_0
            player_1 = '' if player_1 == None else player_1

            res = json.dumps({
                "state" : state,
                "player_0"  : player_0,
                "player_1"  : player_1,
                "player_0_plane": p1_plane,
                "player_1_plane": p2_plane,
                "session" : session,
                "turn" : turn,
                "attack_pos" : attack_pos,
                "win" : win,
                "p0_left_time" : p0_left_time,
                "p1_left_time" : p1_left_time
            })
            
            print(res)
            self.send_response(200,'OK')
            self.send_header('content-length',len(res))
            self.send_header('Connection','close')
            self.end_headers()
            self.flush_headers()

            self.wfile.write(res.encode('utf-8'))
                    
        except:
            self.send_response(500)
            self.end_headers()
            self.flush_headers()
            pass
        
        
    def OnCreateGame(self) ->None:
        length = 0
        try:
            length = int(self.headers['content-length'])
            body = self.rfile.read(length).decode()
            resquest = json.loads(body)
            
            nickname = resquest['nickname']
            password = resquest['password']
            
            session = MyHttpHandler.gameManager.createGame(nickname,password)
            res = json.dumps({
                "result"  : 0,
                "error"   : "success",
                "session" : session
            })
            print(res)
            self.send_response(200,'OK')
            self.send_header('content-length',len(res))
            self.send_header('Connection','close')
            self.end_headers()
            self.flush_headers()

            self.wfile.write(res.encode('utf-8'))
        
        except:
            self.send_response(500)
            self.end_headers()
            self.flush_headers()
            pass
        
    '''
    Put Plane.
    -->
    {
        "session"  : session,
        "nickname" : nickname,
        "password" : password,
        "plane_info" : {
            'idx' : 0,
            'x'   : 0,
            'y'   : 0
        }
    }
    
    <--
    {
        "result"  : 0,
        "error"   : "",
    }
    
    
    
    '''
    def OnRemovePlane(self)->None:
        length = 0
        try:
            length = int(self.headers['content-length'])
            body = self.rfile.read(length).decode()
            resquest = json.loads(body)
            
            session = resquest['session']
            nickname = resquest['nickname']
            password = resquest['password']
            plane_idx = resquest['idx']
            plane_x = resquest['x']
            plane_y = resquest['y']
            
            code,err = MyHttpHandler.gameManager.removePlane(session,nickname,password,plane_idx,plane_x,plane_y)
            
            res = json.dumps({
                "result"  : code,
                "error"   : err
            })

            print(res)
            self.send_response(200,'OK')
            self.send_header('content-length',len(res))
            self.send_header('Connection','close')
            self.end_headers()
            self.flush_headers()

            self.wfile.write(res.encode('utf-8'))
        
        except:
            self.send_response(500)
            self.end_headers()
            self.flush_headers()
            pass
        
    def OnPutPlane(self)->None:
        length = 0
        try:
            length = int(self.headers['content-length'])
            body = self.rfile.read(length).decode()
            resquest = json.loads(body)
            
            session = resquest['session']
            nickname = resquest['nickname']
            password = resquest['password']
            plane_idx = resquest['idx']
            plane_x = resquest['x']
            plane_y = resquest['y']
            
            code,err = MyHttpHandler.gameManager.putPlane(session,nickname,password,plane_idx,plane_x,plane_y)
            
            res = json.dumps({
                "result"  : code,
                "error"   : err
            })

            print(res)
            self.send_response(200,'OK')
            self.send_header('content-length',len(res))
            self.send_header('Connection','close')
            self.end_headers()
            self.flush_headers()

            self.wfile.write(res.encode('utf-8'))
        
        except:
            self.send_response(500)
            self.end_headers()
            self.flush_headers()
            pass
    '''
    Join Game.
    -->
    {
        "nickname" : nickname,
        "password" : password
    }
    
    <--
    {
        "result"  : 0,
        "error"   : "",
        "session" : ""
    }
    '''
    
    def OnJoinGame(self) ->None:
        length = 0
        try:
            length = int(self.headers['content-length'])
            resquest = json.loads(self.rfile.read(length))
            
            nickname = resquest['nickname']
            password = resquest['password']
            session  = resquest['session']
            
            ret = MyHttpHandler.gameManager.joinGame(session,nickname,password)

            res = ''
            
            if ret == -1:
                res = json.dumps({
                    "result"  : -1,
                    "error"   : "invalid session",
                })
            elif ret == -2:
                res = json.dumps({
                    "result"  : -2,
                    "error"   : "game has started",
                    "session" : session
                })
            elif ret == 0:
                res = json.dumps({
                    "result"  : 0,
                    "error"   : "success",
                    "session" : session
                })
            
            self.send_response(200,'OK')
            self.send_header('content-length',len(res))
            self.send_header('Connection','close')
            self.end_headers()
            self.flush_headers()

            self.wfile.write(res.encode('utf-8'))
        except :
            self.send_response(500)
            self.end_headers()
            self.flush_headers()
            pass



def run(ip,port):
    print('start http server')
    address = (ip,port)
    ##http server config
    HTTP_Server = http.server.ThreadingHTTPServer
    HTTP_Handler = MyHttpHandler
    #init handers
    MyHttpHandler._init_handler_map_()
    #instance.
    httpd = HTTP_Server(address,HTTP_Handler)
    httpd.serve_forever()


run("0.0.0.0",7777)