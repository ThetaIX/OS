#include <linux/module.h>
#include <linux/kernel.h>
#include <linux/init.h>
#include <linux/proc_fs.h>
#include <linux/uaccess.h>
#include <linux/time.h>

#define PROC_NAME "tsulab"

static struct proc_dir_entry *proc_file;

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Ilya");
MODULE_DESCRIPTION("TSU kernel module");

static int calculate_minutes_to_alignment(void) {
    struct timespec64 ts;
    struct tm time;
    int hour, minute, minutes_to_next_event;

    ktime_get_real_ts64(&ts);
    time64_to_tm(ts.tv_sec, 0, &time);

    hour = time.tm_hour % 12;
    minute = time.tm_min;

    int relative_speed = 5500; // 5.5 градусов/мин в тысячных долях
    int minute_angle = minute * 6000; // 6 градусов за минуту
    int hour_angle = hour * 30000 + (minute * 500); // 30 градусов/час + 0.5 градуса/мин

    int angle_diff = hour_angle - minute_angle;

    if (angle_diff == 0 || angle_diff == 180000 || angle_diff == -180000) { return 0; }

    if(angle_diff < 0)
    {
        angle_diff += 360000;
    }

    if (angle_diff > 180000) {
        angle_diff = 360000 - angle_diff;
    }

    minutes_to_next_event = (int)(angle_diff/relative_speed);

    
    return minutes_to_next_event;
}

static ssize_t proc_read(struct file *file, char __user *buf, size_t count, loff_t *pos) {
    char buffer[64];
    int len;

    if (*pos > 0) return 0;

    len = snprintf(buffer, sizeof(buffer), "Minutes to alignment: %d\n", calculate_minutes_to_alignment());
    if (copy_to_user(buf, buffer, len)) return -EFAULT;

    *pos = len;
    return len;
}

static const struct proc_ops proc_file_ops = {
    .proc_read = proc_read,
};


// Функция инициализации
static int __init tsu_init(void)
{
    printk(KERN_INFO "Welcome to the Tomsk State University\n");
    proc_file = proc_create(PROC_NAME, 0444, NULL, &proc_file_ops);
    if (!proc_file) {
        printk(KERN_ERR "TSULAB: Failed to create /proc/%s\n", PROC_NAME);
        return -ENOMEM;
    }
    printk(KERN_INFO "TSULAB: Module loaded. File /proc/%s created.\n", PROC_NAME);
    return 0;
}

// Функция выгрузки
static void __exit tsu_exit(void)
{
    proc_remove(proc_file);
    printk(KERN_INFO "TSULAB: Module unloaded. File /proc/%s removed.\n", PROC_NAME);
    printk(KERN_INFO "Tomsk State University forever!\n");
}
module_init(tsu_init);
module_exit(tsu_exit);