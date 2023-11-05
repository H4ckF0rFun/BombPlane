import random
import time
import threading

PLANE_NONE = 0
PLANE_HEAD = 2
PLANE_BODY = 1



# 放飞机时间 2min
# 决策时间   1min

class Player:

    PLANE_0 = [
        [PLANE_NONE,PLANE_NONE,PLANE_HEAD,PLANE_NONE,PLANE_NONE],
        [PLANE_BODY,PLANE_BODY,PLANE_BODY,PLANE_BODY,PLANE_BODY],
        [PLANE_NONE,PLANE_NONE,PLANE_BODY,PLANE_NONE,PLANE_NONE],
        [PLANE_NONE,PLANE_BODY,PLANE_BODY,PLANE_BODY,PLANE_NONE]
    ]
    

    PLANE_1 = [
        [PLANE_NONE,PLANE_NONE,PLANE_BODY,PLANE_NONE],
        [PLANE_BODY,PLANE_NONE,PLANE_BODY,PLANE_NONE],
        [PLANE_BODY,PLANE_BODY,PLANE_BODY,PLANE_HEAD],
        [PLANE_BODY,PLANE_NONE,PLANE_BODY,PLANE_NONE],
        [PLANE_NONE,PLANE_NONE,PLANE_BODY,PLANE_NONE]
    ]
    
    PLANE_2 = [
        [PLANE_NONE,PLANE_BODY,PLANE_BODY,PLANE_BODY,PLANE_NONE],
        [PLANE_NONE,PLANE_NONE,PLANE_BODY,PLANE_NONE,PLANE_NONE],
        [PLANE_BODY,PLANE_BODY,PLANE_BODY,PLANE_BODY,PLANE_BODY],
        [PLANE_NONE,PLANE_NONE,PLANE_HEAD,PLANE_NONE,PLANE_NONE]
    ]
    
    PLANE_3 = [
        [PLANE_NONE,PLANE_BODY,PLANE_NONE,PLANE_NONE],
        [PLANE_NONE,PLANE_BODY,PLANE_NONE,PLANE_BODY],
        [PLANE_HEAD,PLANE_BODY,PLANE_BODY,PLANE_BODY],
        [PLANE_NONE,PLANE_BODY,PLANE_NONE,PLANE_BODY],
        [PLANE_NONE,PLANE_BODY,PLANE_NONE,PLANE_NONE],
    ]
    
    
    def __init__(self) -> None:
        self.board = [[0 for x in range(10)] for y in range(10)]
        self.nickname = None
        self.password  = None
        self.left_plane = 3
        self.attack_pos = []
        self.leave = False
        self.decide_end_time = -1
        
    def setPlayer(self,nickname,password):
        self.nickname = nickname
        self.password = password
        
        
    def getVal(self,x,y):
        if x < 0 or x >= 10:
            return -1
        
        if y < 0 or y >= 10:
            return -1
        
        return self.board[y][x]
    
    def printPlane(self,board):
        for line in board:
            for v in line:
                if v == PLANE_HEAD:
                    print('X ',end='')
                elif v == PLANE_BODY:
                    print('= ',end='')
                elif v == PLANE_NONE:
                    print('  ',end='')
                
            print('')
            
        print("*" * 0x20)
        return
    
    def putPlane(self,x,y,rotate_times):
        if self.left_plane == -1: #confirmed
            return 1
        
        rotate_times = rotate_times % 4
        plane = eval("Player.PLANE_" + str(rotate_times))
        
        width = len(plane[0])
        height = len(plane)
        
        if self.left_plane == 0:
            return -1
        
        if x + width > 10:
            return -1
        
        if y + height > 10:
            return -1
        
        for dy in range(height):
            for dx in range(width):
                _x = x + dx
                _y = y + dy
                
                if plane[dy][dx] != PLANE_NONE:
                    if self.board[_y][_x] != PLANE_NONE:
                        return -1
                    
                    self.board[_y][_x] = plane[dy][dx]
        
        self.left_plane -= 1
        return 0

    def confirm(self):
        if self.left_plane == -1:
            return (0,"success")
        
        if self.left_plane != 0:
            return (1,"left plane is not 0")
        
        self.left_plane = -1
        return (0,"success")
    
    def is_attack(self,x,y):
        if x < 0 or x >= 10:
            return (1,"index out of range (%d,%d)" % (x,y),0)
        
        if y < 0 or y >= 10:
            return (1,"index out of range (%d,%d)" % (x,y),0)
        
        idx = x + y * 10
        if idx in self.attack_pos:
            return (2,"Invalid Position! (%d,%d) idx: %d"%(x,y,idx),0)
        
        value = self.board[y][x]
        
        if self.board[y][x] == PLANE_HEAD:
            self.left_plane -= 1
        
        self.board[y][x] = -1
        self.attack_pos.append(idx)
        
        return (0,"success",value)
        
    def remove_plane(self,x,y,idx):
        if self.left_plane == -1: #confirmed
            return 1
        
        if self.left_plane == 3:
            return 0
        
        idx = idx % 4
        plane = eval("Player.PLANE_" + str(idx))
        
        width = len(plane[0])
        height = len(plane)
        
        if x + width > 10:
            return -1
        
        if y + height > 10:
            return -1
        
        #check is valid plane.        
        for dy in range(height):
            for dx in range(width):
                _x = x + dx
                _y = y + dy
                
                if plane[dy][dx] != PLANE_NONE:
                    if self.board[_y][_x] != plane[dy][dx]:
                        return -1

        for dy in range(height):
            for dx in range(width):
                _x = x + dx
                _y = y + dy
                
                if plane[dy][dx] != PLANE_NONE:
                    self.board[_y][_x] = PLANE_NONE

        self.left_plane += 1
        return 0
                    

GAME_WAIT_JOIN   = 0
GAME_PLACE_PLANE = 1
GAME_START_GUESS = 2
GAME_END         = 3


class Game:
    def __init__(self,session) -> None:
        self.state = 0
        self.turn = 0
        self.player_0 = Player()
        self.player_1 = Player()
        self.session = session
        self.lock = threading.Lock()
        self.win = -1

class GameManager:
    
    def __init__(self) -> None:
        self.games = {}
        self.lock = threading.Lock()
        
    
    def genSession():
        s = random.randbytes(16)
        session = ''
        for byte in s: session += "%0x2" % byte
        return session        
    
    def getInfo(self,session,nickname,password):
        self.lock.acquire()
        game = self.games.get(session)
        self.lock.release()
        
        if game == None:
            return None
        
        game.lock.acquire()
        
        try:    
            if game.player_0.password != None and game.player_0.password == password:
                if game.player_0.nickname != nickname:
                    raise Exception("Invalid User Info")
                
            if game.player_1.password != None and game.player_1.password == password:
                if game.player_1.nickname != nickname:
                    raise Exception("Invalid User Info")
            #
            attack_pos = []
            player = None
            
            if game.player_0.password == password:
                player = game.player_0 
            elif game.player_1.password == password:
                player = game.player_1
            
            attack_pos = player.attack_pos
            
            '''
            
            '''
            #设置player 的剩余决策时间.
            if game.state == GAME_PLACE_PLANE:
                if player.left_plane != -1 and player.decide_end_time == -1:
                    player.decide_end_time = int(time.time()) + 2 * 60

            elif game.state == GAME_START_GUESS:
                ###
                if game.player_0.password == password:
                    if game.turn%2 == 0 and game.player_0.decide_end_time == -1:
                        game.player_0.decide_end_time = int(time.time()) + 1 * 60
                            
                elif game.player_1.password == password:
                    if game.turn%2 == 1 and game.player_1.decide_end_time == -1:
                        game.player_1.decide_end_time = int(time.time()) + 1 * 60
                ###

            ###计算player 是否超时.
            
            player0_left_time = None
            player1_left_time = None
            
            if game.player_0.decide_end_time != -1:
                player0_left_time = game.player_0.decide_end_time - int(time.time())
                
            if game.player_1.decide_end_time != -1:
                player1_left_time = game.player_1.decide_end_time - int(time.time())
            
            if player0_left_time and player0_left_time <= 0:
                game.state = GAME_END
                game.win = 1
                
            elif player1_left_time and player1_left_time <= 0:
                game.state = GAME_END
                game.win = 0
            
            player0_left_time = -1 if player0_left_time == None else player0_left_time
            player1_left_time = -1 if player1_left_time == None else player1_left_time
            
            #
            if game.state == GAME_START_GUESS:
                if  game.player_0.left_plane == 0 or game.player_1.left_plane == 0:
                    game.state = GAME_END
                    
                    if game.player_0.left_plane == 0:
                        game.win = 1
                    elif game.player_1.left_plane == 0:
                        game.win = 0 
            
            if game.state == GAME_END:
                player.leave = True
                if game.player_0.leave == True and game.player_1.leave == True:
                    print("remove session :" + session)
                    self.lock.acquire()
                    self.games.pop(session)
                    self.lock.release()
        
        except Exception as e:
            print(e)
            
        game.lock.release()

        return (game.state,
                session,
                game.player_0.nickname,
                game.player_1.nickname,
                game.player_0.left_plane,
                game.player_1.left_plane,
                game.turn,
                attack_pos,
                game.win,
                player0_left_time,
                player1_left_time)
    

    def putPlane(self,session,nickname,password,plane_idx,x,y):
        self.lock.acquire()
        game = self.games.get(session)
        self.lock.release()
        
        if game == None:
            return (1,"Invalid session")
        
        code,msg = -1,"error"
        
        game.lock.acquire()
        
        try:
            if game.state != GAME_PLACE_PLANE:
                msg = "Not matched state"
                raise Exception("Not matched state")
            
            if game.player_1.nickname == nickname and game.player_1.password == password:
                if game.player_1.putPlane(x,y,plane_idx) != 0:
                    msg = "Invalid Position"
                    raise Exception("Invalid Position")
                
                code = 0
                msg = "success"
            
            elif game.player_0.nickname == nickname and game.player_0.password == password:
                if game.player_0.putPlane(x,y,plane_idx) != 0:
                    msg = "Invalid Position"
                    raise Exception("Invalid Position")

                code = 0
                msg = "success"
                
        except Exception as e:
            print(e)
            
        game.lock.release()
        return code,msg
    
        
    def removePlane(self,session,nickname,password,plane_idx,x,y):
        self.lock.acquire()
        game = self.games.get(session)
        self.lock.release()
        
        if game == None:
            return (1,"Invalid session")
        
        code,msg = -1,"error"
        
        game.lock.acquire()
        try:
            if game.state != GAME_PLACE_PLANE:
                msg = "Not matched state"
                raise Exception("Not matched state")
            
            if game.player_1.nickname == nickname and game.player_1.password == password:
                if game.player_1.remove_plane(x,y,plane_idx) != 0:
                    msg = "Invalid Position"
                    raise Exception("Invalid Position")
                code = 0
                msg = "success"
                
            elif game.player_0.nickname == nickname and game.player_0.password == password:
                if game.player_0.remove_plane(x,y,plane_idx) != 0:
                    msg = "Invalid Position"
                    raise Exception("Invalid Position")
                
                code = 0
                msg = "success"
                
        except Exception as e:
            print(e)
                
        game.lock.release()
        return code,msg
    
    #     not enough values to unpack (expected 3, got 2)
    # {"result": -1, "error": "Invalid Info", "value": 0}
    def attack(self,session,nickname,password,x,y):
        self.lock.acquire()
        game = self.games.get(session)
        self.lock.release()
        
        code ,msg,value = -1,"Invalid Info",0

        if game == None:
            return (1,"Invalid session",0)
        
        game.lock.acquire()
        try:
            if game.state != GAME_START_GUESS:
                msg = "Not matched state"
                raise Exception("Not matched state")
            
            player = None
            
            if game.turn % 2 == 0:
                if game.player_0.nickname != nickname or game.player_0.password != password:
                    msg = "It's not your turn!"
                    raise Exception("It's not your turn!")
                
                attacker = game.player_0
                player = game.player_1

            elif game.turn %2 == 1: 
                if game.player_1.nickname != nickname or game.player_1.password != password:
                    msg = "It's not your turn!"
                    raise Exception("It's not your turn!")
                
                attacker = game.player_1
                player = game.player_0
                
            code ,msg ,value = player.is_attack(x,y)
            
            if code == 0:
                attacker.decide_end_time = -1
                game.turn += 1
                
        except Exception as e:
            print(e)
            
        game.lock.release()
        return code,msg,value
    
    def confirm(self,session,nickname,password):
        self.lock.acquire()
        game = self.games.get(session)
        self.lock.release()
        
        code ,msg = -1,"Invalid Info"

        if game == None:
            return (1,"Invalid session")
        
        game.lock.acquire()
        try:
            
            if game.state != GAME_PLACE_PLANE:
                raise Exception("Not matched state")
            
            player = None
            if game.player_1.nickname == nickname and game.player_1.password == password:
                player = game.player_1
                    
            if game.player_0.nickname == nickname and game.player_0.password == password:
                player = game.player_0
            
            code, msg = player.confirm()
            
            if code == 0 and player.left_plane == -1:
                    player.decide_end_time = -1
                
            if game.player_1.left_plane == -1 and game.player_0.left_plane == -1:
                if game.state == GAME_PLACE_PLANE:
                    game.player_0.left_plane = 3
                    game.player_1.left_plane = 3
                    game.state = GAME_START_GUESS       #开始游戏.
                    game.turn = 0
        except Exception as e:
            print(e)
            
        game.lock.release()
        return code,msg
         
    def createGame(self,nickname,password):
        session = GameManager.genSession()
        game = Game(session)
        
        game.state = GAME_WAIT_JOIN
        game.player_0.setPlayer(nickname,password)
        
        self.lock.acquire()
        self.games[session] = game
        self.lock.release()
        
        return session
    
    def joinGame(self,session,nickname,password):
        self.lock.acquire()
        game = self.games.get(session)
        self.lock.release()
        
        
        if game == None:
            return -1     #invalid session
        
        game.lock.acquire()
        
        try:
            if game.state != GAME_WAIT_JOIN:
                return -2
            
            game.state = GAME_PLACE_PLANE
            game.player_1.nickname = nickname
            game.player_1.password  = password 
            
        except Exception as e:
            print(e)

        game.lock.release()
        return 0
        
        