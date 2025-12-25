import * as THREE from 'three'
import * as TWEEN from '@tweenjs/tween.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';

const CAMERA_PRESETS = {
  top: {
    position: new THREE.Vector3(0, 20, 0.001),
    target: new THREE.Vector3(0, 0, 0),
    fov: 25,
  },
  side: {
    position: new THREE.Vector3(20, 5, 0),
    target: new THREE.Vector3(0, 0, 0),
    fov: 50,
  },
  perspective: {
    position: new THREE.Vector3(6, 6, 6),
    target: new THREE.Vector3(0, 0, 0),
    fov: 60,
  },
}

function drawRoundedRect(
  ctx: CanvasRenderingContext2D,
  x: number,
  y: number,
  w: number,
  h: number,
  r: number
) {
  ctx.beginPath()
  ctx.moveTo(x + r, y)
  ctx.lineTo(x + w - r, y)
  ctx.quadraticCurveTo(x + w, y, x + w, y + r)
  ctx.lineTo(x + w, y + h - r)
  ctx.quadraticCurveTo(x + w, y + h, x + w - r, y + h)
  ctx.lineTo(x + r, y + h)
  ctx.quadraticCurveTo(x, y + h, x, y + h - r)
  ctx.lineTo(x, y + r)
  ctx.quadraticCurveTo(x, y, x + r, y)
  ctx.closePath()
}

export function makeLabel(
  text: string,
  options?: {
    bg?: string
    fg?: string
    padding?: number
  }
) {
  const bg = options?.bg ?? 'rgba(0,0,0,0.6)'
  const fg = options?.fg ?? '#ffffff'

  const canvas = document.createElement('canvas')
  const ctx = canvas.getContext('2d')!
  //const metrics = ctx.measureText(text)

  canvas.width = 512
  canvas.height = 128

  // background
  ctx.fillStyle = bg
  drawRoundedRect(ctx, 0, 0, canvas.width, canvas.height, 14)
  ctx.fill()

  ctx.font = 'bold 80px sans-serif'
  ctx.fillStyle = fg
  ctx.textAlign = 'center'
  ctx.textBaseline = 'middle'
  ctx.fillText(text, canvas.width / 2, canvas.height / 2)

  

  const texture = new THREE.CanvasTexture(canvas)
  texture.minFilter = THREE.LinearFilter

  const material = new THREE.SpriteMaterial({
    map: texture,
    transparent: true,
    depthTest: true
  })

  const sprite = new THREE.Sprite(material)
  sprite.scale.set(canvas.width / 350, canvas.height / 350, 1)

  return sprite
}

export function makeAxis(name: string, bg: string | 'rgba(0,0,0,0.7)', fg: string | '#ffffff') {
    let axis = new THREE.Group()

    let mesh = new THREE.AxesHelper(1);
    mesh.name = 'axis'
    let axislabel = makeLabel(name, {
      bg: bg,
      fg: fg
    })
    axislabel.name = 'label'

    axis.add(mesh, axislabel)
    
    return axis
}

export function transitionToPreset(
  camera: THREE.PerspectiveCamera,
  controls: OrbitControls,
  preset: keyof typeof CAMERA_PRESETS,
  duration = 5
) {
  const p = CAMERA_PRESETS[preset]

  const state = {
    px: camera.position.x,
    py: camera.position.y,
    pz: camera.position.z,
    tx: controls.target.x,
    ty: controls.target.y,
    tz: controls.target.z,
    fov: camera.fov,
  }

  controls.enabled = false // Foxglove disables input mid-transition

  let transistion = new TWEEN.Tween(state)
    .to(
      {
        px: p.position.x,
        py: p.position.y,
        pz: p.position.z,
        tx: p.target.x,
        ty: p.target.y,
        tz: p.target.z,
        fov: p.fov,
      },
      duration
    )
    .easing(TWEEN.Easing.Cubic.InOut)
    .onUpdate(() => {
      camera.position.set(state.px, state.py, state.pz)
      controls.target.set(state.tx, state.ty, state.tz)
      camera.fov = state.fov
      camera.updateProjectionMatrix()
      controls.update()
    })
    .onComplete(() => {
      controls.enabled = true
    })
    .start()
  transistion.update(800)
}