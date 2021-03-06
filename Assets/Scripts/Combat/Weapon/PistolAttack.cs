﻿using UnityEngine;

class PistolAttack : BaseAttack
{
    public GameObject bulletSpark = null;
    private bool isWoman;

    protected override void PrepareToLightAttack()
    {
        isWoman = name.Contains("Woman");
        base.PrepareToLightAttack();
        Aim();
    }

    protected override void PrepareToHeavyAttack()
    {
        isWoman = name.Contains("Woman");
        base.PrepareToHeavyAttack();
        Aim();
    }

    protected override void PerformLightAttack()
    {
        if (!IsAttacking()) return;

        Shoot();
        state = State.Perform;
        Invoke("FinishLightAttack", lightAttackTime);
    }

    protected override void PerformHeavyAttack()
    {
        if (!IsAttacking()) return;

        Shoot();
        state = State.Perform;
        Invoke("FinishHeavyAttack", lightAttackTime);
    }

    protected override void FinishLightAttack()
    {
        if (!IsAttacking()) return;

        base.FinishLightAttack();
        Reload();
    }

    protected override void FinishHeavyAttack()
    {
        if (!IsAttacking()) return;

        base.FinishHeavyAttack();
        Reload();
    }

    private void Aim()
    {
        if (isWoman)
            animator.TransitionPlay("Woman Draw Pistol");
        else
            animator.TransitionPlay("Draw Pistol");
    }

    private void Shoot()
    {
        if (isWoman)
            animator.TransitionPlay("Woman Shoot Pistol");
        else
            animator.TransitionPlay("Shoot Pistol");
        SoundPlayer.Play("Pistol Fire");
        if (weapon.GetComponent<MusketFire>()) weapon.GetComponent<MusketFire>().Fire();
        ShootCollisionCheck();
    }

    private void Reload()
    {
        if (isWoman)
            animator.TransitionPlay("Woman Reload Pistol");
        else
            animator.TransitionPlay("Reload Pistol");
        SoundPlayer.Play("Pistol Reload");
    }

    private void ShootCollisionCheck()
    {
        Vector2 direction = Vector2.right;
        if (GetComponent<Movement>())
            if (GetComponent<Movement>().direction == Movement.Direction.Left)
                direction = Vector2.left;
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one, 0, direction, 200, _collision.collisionLayer);
        if (hit)
        {
            Damage damage = hit.collider.GetComponent<Damage>();
            Stun stun = hit.collider.GetComponent<Stun>();
            if (damage)
            {
                damage.ExecuteDamage(attack.GetDamageAmount(), null);
                Object blood = Instantiate(damage.bloodShoot, hit.point, Quaternion.identity);
                if (transform.localScale.x < 0) ((GameObject)blood).transform.Rotate(0, 180, 0);
            }
            if (stun)
                stun.GetStunned(stunAmount: 0.7f, power: Stun.Power.Shoot);
            if (bulletSpark)
            {
                GameObject instance = Instantiate(bulletSpark);
                instance.transform.position = new Vector3(hit.point.x, hit.point.y, -35);
                if (transform.localScale.x > 0)
                {
                    Vector3 euler = instance.transform.localEulerAngles;
                    euler.y = 180;
                    instance.transform.localEulerAngles = euler;
                }
                if (!hit.collider.tag.Contains("Obstacle"))
                    instance.transform.FindContainsInChildren("Wood").SetActive(false);
            }
        }
    }
}