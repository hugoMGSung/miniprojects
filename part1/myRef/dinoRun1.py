# dino Run
import pygame
import os

pygame.init()

# 전역변수
SCREEN_HEIGHT = 600
SCREEN_WIDTH = 1100
SCREEN = pygame.display.set_mode((SCREEN_WIDTH, SCREEN_HEIGHT))

RUNNING = [pygame.image.load(os.path.join('./myRef/Assets/Dino', 'DinoRun1.png')),
           pygame.image.load(os.path.join('./myRef/Assets/Dino', 'DinoRun2.png'))]
JUMPING = pygame.image.load(os.path.join('./myRef/Assets/Dino', 'DinoJump.png'))
DUCKING = [pygame.image.load(os.path.join('./myRef/Assets/Dino', 'DinoDuck1.png')),
           pygame.image.load(os.path.join('./myRef/Assets/Dino', 'DinoDuck2.png'))]

SMALL_CACTUS = [pygame.image.load(os.path.join('./myRef/Assets/Cactus', 'SmallCactus1.png')),
                pygame.image.load(os.path.join('./myRef/Assets/Cactus', 'SmallCactus2.png')),
                pygame.image.load(os.path.join('./myRef/Assets/Cactus', 'SmallCactus3.png'))]
LARGE_CACTUS = [pygame.image.load(os.path.join('./myRef/Assets/Cactus', 'LargeCactus1.png')),
                pygame.image.load(os.path.join('./myRef/Assets/Cactus', 'LargeCactus2.png')),
                pygame.image.load(os.path.join('./myRef/Assets/Cactus', 'LargeCactus3.png'))]

BIRD = [pygame.image.load(os.path.join('./myRef/Assets/Bird', 'Bird1.png')),
        pygame.image.load(os.path.join('./myRef/Assets/Bird', 'Bird2.png'))]

CLOUD = pygame.image.load(os.path.join('./myRef/Assets/Other', 'Cloud.png'))

BG = pygame.image.load(os.path.join('./myRef/Assets/Other', 'Track.png'))

class Dino:
    X_POS = 80
    Y_POS = 310

    def __init__(self) -> None:
        self.run_img = RUNNING
        self.duck_img = DUCKING
        self.jump_img = JUMPING

        self.dino_run = True
        self.dino_duck = False
        self.dino_jump = False

        self.step_index = 0
        self.image = self.run_img[0]
        self.dino_rect = self.image.get_rect()
        self.dino_rect.x = self.X_POS
        self.dino_rect.y = self.Y_POS

    def update(self, userInput):
        if self.dino_run:
            self.run()
        elif self.dino_duck:
            self.duck()
        elif self.dino_jump:
            self.jump()

        if self.step_index >= 10:
            self.step_index = 0

        if userInput[pygame.K_UP] and not self.dino_jump:
            self.dino_run = False
            self.dino_duck = False
            self.dino_jump = True
        elif userInput[pygame.K_DOWN] and not self.dino_jump:
            self.dino_run = False
            self.dino_duck = True
            self.dino_jump = False
        elif not (self.dino_jump or userInput[pygame.K_DOWN]):
            self.dino_run = True
            self.dino_duck = False
            self.dino_jump = False

    def run(self):
        self.image = self.run_img[self.step_index // 5]
        self.dino_rect = self.image.get_rect()
        self.dino_rect.x = self.X_POS
        self.dino_rect.y = self.Y_POS
        self.step_index += 1

    def duck(self):
        pass

    def jump(self):
        pass

    def draw(self, SCREEN):
        SCREEN.blit(self.image, (self.dino_rect.x, self.dino_rect.y))


def main():
    run = True
    clock = pygame.time.Clock()
    player = Dino()

    while run:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                run = False

        SCREEN.fill((255, 255, 255))
        userInput = pygame.key.get_pressed()

        player.draw(SCREEN)
        player.update(userInput)

        clock.tick(30)
        pygame.display.update()


if __name__ == '__main__':
    main()        