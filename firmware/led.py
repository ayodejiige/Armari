from external import HT16K33
import time
import argparse

class LedController(HT16K33.HT16K33):
    _N_ANODES = 16
    def __init__(self, **kwargs):
        super(LedController, self).__init__(**kwargs)
        self.begin()
        self.write_display()

    def reset(self):
        self.clear()
        self.write_display()

    def set_pixel(self, x, y, value):
        """Set pixel at position x, y to the given value.  X and Y should be values
        of 0 to 7.  Value should be 0 for off and non-zero for on.
        """
        if x < 0 or x > 1 or y < 0 or y > 3:
            raise ValueError('LED must be value of 0 to 7.')
        led = y*self._N_ANODES + x
        self.set_led(led ,value)
        self.write_display()

led_states = ["on", "off"]
def set_led(args):
    x = args.pos[0]
    y = args.pos[1]
    value = args.value[0]
    print (value)
    print(args)
    print ("Setting led at row %d, col %d %s" %(x, y, led_states[value]))
    led = LedController()
    led.set_pixel(x, y, value)

def clear_all(args):
    print ("Clearing all leds")
    led = LedController()
    led.reset()

def main():
    parser = argparse.ArgumentParser(description='Process some integers.')
    commands = parser.add_subparsers(title='sub-commands')
    set_parser = commands.add_parser('set', help='set an led')
    set_parser.add_argument('--pos', '-p', type=int, nargs=2,
                        default=[0, 0],
                        help='led location (e.g. --led <row> <col>)')
    set_parser.add_argument('--value', '-v', type=int, nargs=1,
                        default=0,
                        help='1 to turn on, 0 to tuen off')
    set_parser.set_defaults(func=set_led)
    clear_parser = commands.add_parser('clear', help='clear all leds')
    clear_parser.set_defaults(func=clear_all)
    
    args = parser.parse_args()
    args.func(args)

if __name__ == "__main__":
    main()
    