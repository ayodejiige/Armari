from external import HT16K33
import time

class LedController(HT16K33.HT16K33):
    _N_ROWS = 4
    _N_COLS = 2
    _N_ANODES = 16
    # ROW_IO_MAP = {}
    # COL_IO_MAP = {}
    def __init__(self, **kwargs):
        super(LedController, self).__init__(**kwargs)
        self.begin()
        self.write_display()

    def reset():
        self.clear()
        self.write_display()

    def set_pixel(self, x, y, value):
        """Set pixel at position x, y to the given value.  X and Y should be values
        of 0 to 7.  Value should be 0 for off and non-zero for on.
        """
        if x < 0 or x > 1 or y < 0 or y > 3:
            raise ValueError('LED must be value of 0 to 7.')
        led = x*self._N_ANODES + 7
        self.set_led(led ,value)
        self.write_display()


def main():
    led = LedController()
    led.clear()
    led.write_display()
    time.sleep(2)
    loc = [0, 1, 16, 17]
    while(1):
        for i in loc:
            print ("Setting led %d" %i)
            led.set_led(i, 1)
            led.write_display()
            time.sleep(0.5)
            led.set_led(i, 0)
            led.write_display()

if __name__ == "__main__":
    main()
    