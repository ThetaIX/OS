#include <linux/module.h>
#include <linux/kernel.h>
#include <linux/init.h>

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Ilya");
MODULE_DESCRIPTION("TSU kernel module");

// Функция инициализации
static int __init tsu_init(void)
{
    printk(KERN_INFO "Welcome to the Tomsk State University\n");
    return 0;
}

// Функция выгрузки
static void __exit tsu_exit(void)
{
    printk(KERN_INFO "Tomsk State University forever!\n");
}

module_init(tsu_init);
module_exit(tsu_exit);


// через сколько минут стрелки часов выстроятся в одну линию