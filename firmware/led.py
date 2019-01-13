class LedController(object):
    N_ROWS = 4
    N_COLS = 4
    ROW_IO_MAP = {}
    COL_IO_MAP = {}
    def __init__(self):
        self._matrix = [[0]*N_COLS]*N_ROWS
    
    def set_led(self, *loc):
        pass
    
    def turn_on(self):
        pass

    def turn_off(self):
        pass
    
    def run(self):
        pass


def main():
    pass

if __name__ == "__main__":
    main()
    